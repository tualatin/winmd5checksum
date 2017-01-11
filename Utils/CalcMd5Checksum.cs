using Org.Vs.WinMd5Checksum.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;


namespace Org.Vs.WinMd5Checksum.Utils
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
      workerThread = new Thread(WorkerThread)
      {
        Name = "CalcFileHashes",
        IsBackground = true
      };

      isRunning = true;
      workerThread.Start();

      Thread.Sleep(1);

      if (isRunning == false)
        StopWorkerThread();
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
        workerThread.Abort();
    }

    /// <summary>
    /// Read md5 file
    /// </summary>
    /// <param name="fileName">file and path</param>
    /// <returns>the hash string</returns>
    public static string ReadMd5File (string fileName)
    {
      string expectedChecksum;

      using (StreamReader sr = new StreamReader(fileName))
      {
        expectedChecksum = sr.ReadLine();
      }

      if (expectedChecksum != null && !string.IsNullOrEmpty(expectedChecksum.Trim()))
        return (expectedChecksum.Trim());

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
          StartCalculation();
        }
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog(ErrorFlags.Error, "CalcMd5Checksum", string.Format("WorkerThread, exception: {0}", ex));
      }
    }

    /// <summary>
    /// Stops main thread
    /// </summary>
    private static void StopWorkerThread ()
    {
      isRunning = false;
      workerThread.Join();
    }

    /// <summary>
    /// Start calcultion
    /// </summary>
    private static void StartCalculation ()
    {
      DateTime now = DateTime.Now;
      List<Md5Structure> compareRest = new List<Md5Structure>();

      System.Threading.Tasks.Parallel.For(0, Md5Files.GetFileContainer.Count, i =>
     {
       Md5Structure item = Md5Files.GetFileContainer[i];

       if (File.Exists(item.key))
       {
         FileStream fs = new FileStream(item.key, FileMode.Open, FileAccess.Read);

         if (string.IsNullOrEmpty(item.calc))
           item.calc = GetMd5HashOf(fs);
         if (!string.IsNullOrEmpty(item.compare))
         {
           item.result = DoCompare(item.calc, item.compare);
           compareRest.Add(item);
         }

         fs.Close();
         fs = new FileStream(item.key, FileMode.Open, FileAccess.Read);

         if (string.IsNullOrEmpty(item.sha256hash))
           item.sha256hash = GetSha256HashOf(fs);
         if (!string.IsNullOrEmpty(item.compare256hash))
           item.result256compare = DoCompare(item.sha256hash, item.compare256hash);

         fs.Close();
       }
     });

      if (compareRest.Count != Md5Files.GetFileContainer.Count)
      {
        if (compareRest.Count == 1)
          CompareToRest(compareRest[0]);
      }

      if (CanWriteFile)
        LogFile.WriteFile(Md5Files.GetFileContainer);

      TimeSpan timeSpan = DateTime.Now.Subtract(now);
      ErrorLog.WriteLog(ErrorFlags.Info, "CalcMd5Checksum", string.Format("Calculation time: {0}", timeSpan.ToString(@"hh\:mm\:ss\.fff")));
      isRunning = false;
    }

    #endregion

    private static void CompareToRest (Md5Structure item)
    {
      Md5Files.GetFileContainer.ForEach(file => file.result = DoCompare(file.calc, item.compare));
    }

    private static string DoCompare (string result, string expected)
    {
      return (String.Compare(expected, result, StringComparison.Ordinal)) == 0 ? ("OK") : ("FAILED");
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
        if (!File.Exists(fileName))
          throw new FileNotFoundException(string.Format("file {0} not found", fileName));

        string expectedHash;

        using (StreamReader sr = new StreamReader(fileName))
        {
          expectedHash = sr.ReadLine();
        }

        return expectedHash != null && string.IsNullOrEmpty(expectedHash.Trim()) ? (string.Empty) : (expectedHash);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog(ErrorFlags.Error, "CalcMd5Checksum", string.Format("GetValueFromHashFile, exception: {0}", ex));

        return (string.Empty);
      }
    }

    private static string GetMd5HashOf (Stream input)
    {
      return (HashOf<MD5CryptoServiceProvider>(input));
    }

    private static string GetSha256HashOf (Stream input)
    {
      return (HashOf<SHA256CryptoServiceProvider>(input));
    }

    //private static string GetSha512HashOf (Stream input)
    //{
    //  return (HashOf<SHA512CryptoServiceProvider> (input));
    //}

    //private static string HashOf<T> (byte[] bArray)
    //  where T: HashAlgorithm, new ( )
    //{
    //  var provider = new T ( );
    //  var hashValue = provider.ComputeHash (bArray);

    //  return (BitConverter.ToString (hashValue).Replace ("-", string.Empty).ToLower ( ));
    //}

    private static string HashOf<T> (Stream input)
     where T : HashAlgorithm, new()
    {
      const int bufferSize = 1024 * 1024 * 20;
      string hashString;
      System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();

      using (var provider = new T())
      {
        int readCount;
        long bytesTransfered = 0;
        var buffer = new byte[bufferSize];

        while ((readCount = input.Read(buffer, 0, buffer.Length)) != 0)
        {
          if (bytesTransfered + readCount == input.Length)
            provider.TransformFinalBlock(buffer, 0, readCount);
          else
            provider.TransformBlock(buffer, 0, bufferSize, buffer, 0);

          bytesTransfered += readCount;

#if DEBUG
          Console.WriteLine(@"HashOf<{3}> {0}MB/{1}MB. Memory Used: {2}MB", bytesTransfered / 1000000, input.Length / 1000000, process.PrivateMemorySize64 / 1000000, provider.ToString());
#endif
        }

        hashString = BitConverter.ToString(provider.Hash).Replace("-", string.Empty).ToLower();
        provider.Clear();
      }
      return (hashString);
    }
  }
}
