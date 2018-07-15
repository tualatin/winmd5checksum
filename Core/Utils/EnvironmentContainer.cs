using System;
using System.Windows;
using Microsoft.Win32;
using Org.Vs.WinMd5Checksum.Core.Interfaces;


namespace Org.Vs.WinMd5Checksum.Core.Utils
{
  /// <summary>
  /// EnvironmentContainer
  /// </summary>
  public class EnvironmentContainer
  {
    private static EnvironmentContainer instance;

    /// <summary>
    /// Current instance
    /// </summary>
    public static EnvironmentContainer Instance => instance ?? (instance = new EnvironmentContainer());

    /// <summary>
    /// Current event manager
    /// </summary>
    public readonly IEventAggregator CurrentEventManager;

    private EnvironmentContainer()
    {
      CurrentEventManager = new EventAggregator();
    }

    /// <summary>
    /// Application title
    /// </summary>
    public static string ApplicationTitle => Application.Current.TryFindResource("ApplicationTitle").ToString();

    /// <summary>
    /// Application release URL
    /// </summary>
    public static string ApplicationReleaseWebUrl => "https://github.com/tualatin/winmd5checksum/releases";

    /// <summary>
    /// Application donate web URL
    /// </summary>
    public static string ApplicationDonateWebUrl => "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=M436BDAMQL7WE";

    /// <summary>
    /// Current installed .NET version
    /// </summary>
    public static int NetFrameworkKey
    {
      get
      {
        using ( var netFrameworkKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\") )
        {
          try
          {
            return Convert.ToInt32(netFrameworkKey?.GetValue("Release"));
          }
          catch
          {
            return -1;
          }
        }
      }
    }
  }
}
