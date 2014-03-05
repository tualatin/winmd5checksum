using System.IO;
using System;
using System.Windows;
using System.Collections.Generic;
using WinMd5Checksum.Utils;


namespace WinMd5Checksum.Data
{
  public static class LogFile
  {
    private readonly static string path = Path.GetDirectoryName (System.Diagnostics.Process.GetCurrentProcess ( ).MainModule.FileName);
    private readonly static string moduleName = Path.GetFileName (System.Diagnostics.Process.GetCurrentProcess ( ).MainModule.FileName);
    private readonly static string listFile = string.Format ("{0}\\{1}.lst", path, moduleName);
    private static bool shortPrint;
    private static bool listPrint;
    private const string applicationCaption = "WinMD5Checksum";


    public static void SetShortPrint (bool s)
    {
      shortPrint = s;
    }

    public static bool IsShortPrint ()
    {
      return (shortPrint);
    }

    public static void SetListPorint (bool s)
    {
      listPrint = s;
    }

    public static bool IsListPrint ()
    {
      return (listPrint);
    }

    public static void DeleteFile ( ) 
    {
      try
      {
        if (File.Exists (listFile))
          File.Delete (listFile);
      }
      catch (Exception ex)
      {
        Console.Write (string.Format ("DeleteFile exception {0}", ex.Message));
      }
    }

    public static bool WriteSaveFile (string fileName, string hashSum, string shaSum)
    {
      try
      {
        string name = string.Format ("{0}\\{1}.sha256", Path.GetDirectoryName (fileName), Path.GetFileNameWithoutExtension (fileName));

        if (File.Exists (name))
          File.Delete (name);
        if (File.Exists (fileName))
          File.Delete (fileName);

        File.AppendAllText (fileName, hashSum);
        File.AppendAllText (name, shaSum);
        
        return (true);
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "LogFiles", string.Format ("{0} {2}, exception: {1}", System.Reflection.MethodBase.GetCurrentMethod ( ).Name, ex, fileName));

        return (false);
      }
    }

    public static void WriteFile (List<Md5Structure> list)
    {
      try
      {
        string line = string.Empty;
        DateTime now = DateTime.Now;

        if (listPrint == true)
        {
          if (shortPrint == true)
          {
            list.ForEach (item => line += string.Format ("#################\n{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}", item.key, item.calc, item.compare, item.result, item.sha256hash, item.compare256hash, item.result256compare));
          }
          else
          {
            list.ForEach (item => line += string.Format ("#################\nDate: {0}\nFile: {1}\nMd5Calculation: {2}\nMd5ToCompare: {3}\nResult: {4}\nSHA256Hash: {5}\nSHA256HashToCompare: {6}\nResult{7}", now, item.key, item.calc, item.compare, item.result, item.sha256hash, item.compare256hash, item.result256compare));
          }

          File.AppendAllText (listFile, line);
        }
        else
        {
          list.ForEach (item =>
          {
            string md5File = string.Format ("{0}\\{1}.md5", path, Path.GetFileNameWithoutExtension (item.file));
            string sha256File = string.Format ("{0}\\{1}.sha256", path, Path.GetFileNameWithoutExtension (item.file));

            if (File.Exists (md5File))
              File.Delete (md5File);
            if (File.Exists (sha256File))
              File.Delete (sha256File);
            
            if (string.IsNullOrEmpty (item.compare))
            {
              File.AppendAllText (md5File, item.calc);
              File.AppendAllText (sha256File, item.sha256hash);
            }
            else
            {
              line += string.Format ("#################\nDate: {0}\nFile: {1}\nMd5Calculation: {2}\nMd5ToCompare: {3}\nResult: {4}\nSHA256Hash: {5}\nSHA256HashToCompare\nResult: {6}", now, item.key, item.calc, item.compare, item.result, item.sha256hash, item.compare256hash, item.result256compare);

              File.AppendAllText (listFile, line);
            }
          });
        }
      }
      catch (Exception ex)
      {
        ErrorLog.WriteLog (ErrorFlags.Error, "LogFile", string.Format ("{0} (list), exception: {1}", System.Reflection.MethodBase.GetCurrentMethod ( ).Name, ex));
      }
    }

    public static string ApplicationCaption ()
    {
      return (applicationCaption);
    }

    public static void ShowHelp ()
    {
      string appName = Path.GetFileName (Environment.GetCommandLineArgs ( )[0]);

      MessageBox.Show (string.Format ("Application Help:\n\n/q\tQuiet mode, all results will save in md5/sha256 file\n/s\tShort mode, only hash sums will save in md5 file\n\nExamples:\n\n{0} /q /s example.exe\n{0} /s example.iso", appName), applicationCaption, MessageBoxButton.OK, MessageBoxImage.Information);
    }
  }
}
