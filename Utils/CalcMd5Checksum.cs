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
    private static bool writeFile;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public static void SetWriteFile (bool s)
    {
      writeFile = s;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>writeFile</returns>
    public static bool CanWriteFile ()
    {
      return (writeFile);
    }

    /// <summary>
    /// 
    /// </summary>
    public static void CalcMd5HashSum ()
    {
      workerThread = new Thread (WorkerThread)
      {
        Name = "CalcMd5HashSum",
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
    /// 
    /// </summary>
    /// <returns></returns>
    public static Thread GetThread ()
    {
      return (workerThread);
    }

    /// <summary>
    /// 
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

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    private static void StopWorkerThread ()
    {
      isRunning = false;
      workerThread.Join ( );
    }

    private static void StartCalculation ()
    {
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

      if (writeFile == true)
        LogFile.WriteFile (Md5Files.GetFileContainer ( ));

      isRunning = false;
    }

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
          MD5 md5 = new MD5CryptoServiceProvider ( );
          byte[] retVal = md5.ComputeHash (file);
          StringBuilder sb = new StringBuilder ( );

          file.Close ( );

          for (int i = 0; i < retVal.Length; i++)
            sb.Append (retVal[i].ToString ("x2"));

          return (sb.ToString ( ));
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
        SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider ( );
        byte[] arrayData = Encoding.ASCII.GetBytes (fileName);
        byte[] arrayResult = SHA256.ComputeHash (arrayData);
        string result = string.Empty;
        string temp = string.Empty;

        for (int i = 0; i < arrayResult.Length; i++)
        {
          temp = Convert.ToString (arrayResult[i], 16);

          if (temp.Length == 1)
            temp = "0" + temp;

          result += temp;
        }
        return (result);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "CalcMd5Checksum", string.Format ("GetSHA256HashFromFile, exception: {0}", ex));

        return (string.Empty);
      }
    }

    private static string GetMd5HashFromFile (string fileName)
    {
      return (HashOf<MD5CryptoServiceProvider> (fileName, Encoding.Default));
    }

    private static string GetSha256HashFromFile (string fileName)
    {
      return (HashOf<SHA1CryptoServiceProvider> (fileName, Encoding.Default));
    }

    private static string GetSha512HashFromFile (string fileName)
    {
      return (HashOf<SHA512CryptoServiceProvider> (fileName, Encoding.Default));
    }

    private static string HashOf<T> (string text, Encoding enc)
      where T : HashAlgorithm, new ()
    {
      var buffer = enc.GetBytes (text);
      var provider = new T ( );

      return (BitConverter.ToString (provider.ComputeHash (buffer)).Replace ("-", ""));
    }
  }
}
