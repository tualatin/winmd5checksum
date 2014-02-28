using System.Windows;
using WinMd5Checksum.Data;
using WinMd5Checksum.Utils;
using System;


namespace WinMd5Checksum
{
  /// <summary>
  /// Interaction logic for "App.xaml"
  /// </summary>
  public partial class App: Application
  {
    void app_Startup (object Sender, StartupEventArgs e)
    {
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      LogFile.DeleteFile ( );

      if (e.Args.Length == 0)
        return;

      bool quiet = false;

      foreach (string arg in e.Args)
      {
        if (arg.CompareTo ("/q") == 0)
        {
          quiet = true;
          continue;
        }

        if (arg.CompareTo ("/s") == 0)
        {
          LogFile.SetShortPrint (true);
          continue;
        }

        if (arg.CompareTo ("/?") == 0)
        {
          LogFile.ShowHelp ( );
          break;
        }

        if (arg.CompareTo ("/l") == 0)
        {
          LogFile.SetListPorint (true);
          continue;
        }

        Md5Files.AddFileToContainer (arg);
      }

      Md5Files.FinishOperation ( );

      if (quiet != true)
        return;

      CalcMd5Checksum.SetWriteFile (true);
      CalcMd5Checksum.CalcMd5HashSum ( );
      Application.Current.Shutdown ( );
    }

    protected override void OnStartup (StartupEventArgs e)
    {
      ShutdownMode = ShutdownMode.OnExplicitShutdown;

      base.OnStartup (e);
    }

    private static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
    {
      ErrorLog.WriteLog (ErrorFlags.Error, "winmd5checksum", string.Format ("UnhandledException: {0}", e.ExceptionObject));
    }
  }
}
