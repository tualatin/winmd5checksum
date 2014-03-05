using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System;
using WinMd5Checksum.Data;
using System.Collections.Generic;


namespace WinMd5Checksum.Utils
{
  public static class CalcMd5Checksum
  {
    private static volatile bool isRunning;
    private static Thread workerThread;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public static void SetWriteFile (bool s)
    {
      CanWriteFile = s;
    }

    /// <summary>
    /// has file write access
    /// </summary>
    public static bool CanWriteFile
    {
      get;
      private set;
    }

    /// <summary>
    /// Main thread function
    /// </summary>
    public static void CalcMd5HashSum ()
    {
      workerThread = new Thread (WorkerThread)
      {
        Name = "CalcFileHashes",
        IsBackground = true
      };

      isRunning = true;
      workerThread.Start ( );

      while (!workerThread.IsAlive)
        ;

      Thread.Sleep (1);

      if (isRunning == false)
        StopWorkerThread ( );
    }

    /// <summary>
    /// Get main thread
    /// </summary>
    public static Thread GetThread
    {
      get
      {
        return (workerThread);
      }
    }

    /// <summary>
    /// On exit event function
    /// </summary>
    public static void OnExit ()
    {
      if (workerThread.ThreadState != ThreadState.Stopped)
        workerThread.Abort ( );
    }

    /// <summary>
    /// Read md5 file
    /// </summary>
    /// <param name="fileName">file and path</param>
    /// <returns>the hash string</returns>
    public static string ReadMd5File (string fileName)
    {
      string expectedChecksum = string.Empty;

      using (StreamReader sr = new StreamReader (fileName))
      {
        expectedChecksum = sr.ReadLine ( );
      }

      if (!string.IsNullOrEmpty (expectedChecksum.Trim ( )))
        return (expectedChecksum.Trim ( ));
      else
        return (null);
    }

    #region Main thread

    /// <summary>
    /// Starts main thread
    /// </summary>
    private static void WorkerThread ()
    {
      try
      {
        while (isRunning)
        {
          StartCalculation ( );
        }
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "CalcMd5Checksum", string.Format ("WorkerThread, exception: {0}", ex));
      }
    }

    /// <summary>
    /// Stops main thread
    /// </summary>
    private static void StopWorkerThread ()
    {
      isRunning = false;
      workerThread.Join ( );
    }

    /// <summary>
    /// Start calcultion
    /// </summary>
    private static void StartCalculation ()
    {
      DateTime now = DateTime.Now;
      List<Md5Structure> compareRest = new List<Md5Structure> ( );

      Md5Files.GetFileContainer ( ).ForEach (item =>
          {
            if (string.IsNullOrEmpty (item.calc))
              item.calc = GetMd5HashFromFile (item.key);
            if (!string.IsNullOrEmpty (item.compare))
            {
              item.result = DoCompare (item.calc, item.compare);
              compareRest.Add (item);
            }
            if (string.IsNullOrEmpty (item.sha256hash))
              item.sha256hash = GetSHA256HashFromFile (item.key);
            if (!string.IsNullOrEmpty (item.compare256hash))
              item.result256compare = DoCompare (item.sha256hash, item.compare256hash);
          });

      if (compareRest.Count != Md5Files.GetFileContainer ( ).Count)
      {
        if (compareRest.Count == 1)
          CompareToRest (compareRest[0]);
      }

      if (CanWriteFile)
        LogFile.WriteFile (Md5Files.GetFileContainer ( ));

      TimeSpan timeSpan = DateTime.Now.Subtract (now);
      ErrorLog.WriteLog (ErrorFlags.Info, "CalcMd5Checksum", string.Format ("Calculation time: {0}", timeSpan.ToString (@"hh\:mm\:ss\.fff")));
      isRunning = false;
    }

    #endregion

    private static void CompareToRest (Md5Structure item)
    {
      Md5Files.GetFileContainer ( ).ForEach (file => file.result = DoCompare (file.calc, item.compare));
    }

    private static string DoCompare (string result, string expected)
    {
      if ((expected.CompareTo (result)) == 0)
        return ("OK");
      else
        return ("FAILED");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetValueFromHashFile (string fileName)
    {
      try
      {
        if (!File.Exists (fileName))
          throw new FileNotFoundException (string.Format ("file {0} not found", fileName));

        string expectedHash = string.Empty;

        using (StreamReader sr = new StreamReader (fileName))
        {
          expectedHash = sr.ReadLine ( );
        }

        if (string.IsNullOrEmpty (expectedHash.Trim ( )))
          return (string.Empty);

        return (expectedHash);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "CalcMd5Checksum", string.Format ("GetValueFromHashFile, exception: {0}", ex));

        return (string.Empty);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetMd5HashFromFile (string fileName)
    {
      try
      {
        if (File.Exists (fileName))
        {
          FileStream file = new FileStream (fileName, FileMode.Open);
          string hash = GetMd5HashOf (file);
          file.Close ( );

          return (hash);
        }
        else
          return (null);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "CalcMd5Checksum", string.Format ("GetMd5HashFromFile, exception: {0}", ex));

        return (string.Empty);
      }
    }

    private static string GetSHA256HashFromFile (string fileName)
    {
      try
      {
        FileStream filestream = new FileStream (fileName, FileMode.Open);
        string hash = GetSha256HashOf (filestream);
        filestream.Close ( );

        return (hash);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "CalcMd5Checksum", string.Format ("GetSHA256HashFromFile, exception: {0}", ex));

        return (string.Empty);
      }
    }

    private static string GetMd5HashOf (FileStream fileName)
    {
      return (HashOf<MD5CryptoServiceProvider> (fileName));
    }

    private static string GetSha256HashOf (FileStream fileName)
    {
      return (HashOf<SHA256CryptoServiceProvider> (fileName));
    }

    private static string GetSha512HashOf (FileStream fileName)
    {
      return (HashOf<SHA512CryptoServiceProvider> (fileName));
    }

    private static string HashOf<T> (FileStream file)
      where T : HashAlgorithm, new ()
    {
      var provider = new T ( );
      var hashValue = provider.ComputeHash (file);

      return (BitConverter.ToString (hashValue).Replace ("-", string.Empty).ToLower ( ));
    }
  }
}
