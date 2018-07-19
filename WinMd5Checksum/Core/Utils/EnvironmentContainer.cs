using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Org.Vs.WinMd5.Controllers;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Interfaces;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.Data.Messages;
using Org.Vs.WinMd5.UI.UserControls;


namespace Org.Vs.WinMd5.Core.Utils
{
  /// <summary>
  /// EnvironmentContainer
  /// </summary>
  public class EnvironmentContainer : NotifyMaster
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

    private readonly ICalculateHash _calculateHashsumController;
    private readonly ISaveHashToFile _saveHashToFileController;

    private EnvironmentContainer()
    {
      UpTime = DateTime.Now;
      CurrentEventManager = new EventAggregator();
      _calculateHashsumController = new CalculateHash();
      _saveHashToFileController = new SaveHashToFile();
    }

    #region StatusBar default settings

    /// <summary>
    /// Default value StatusBarInactiveBackgroundColor
    /// </summary>
    public const string StatusBarInactiveBackgroundColor = "#FF68217A";

    /// <summary>
    /// Default value StatusBarBusyBackgroundColor
    /// </summary>
    public const string StatusBarBusyBackgroundColor = "#FFCA5100";

    #endregion

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
    /// UserSettings path
    /// </summary>
    public static string UserSettingsPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{ApplicationTitle}";

    private bool _alwaysOnTop;

    /// <summary>
    /// Always on top
    /// </summary>
    public bool AlwaysOnTop
    {
      get => _alwaysOnTop;
      set
      {
        if ( value == _alwaysOnTop )
          return;

        _alwaysOnTop = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Current up time
    /// </summary>
    public DateTime UpTime
    {
      get;
    }

    /// <summary>
    /// Converts a <see cref="System.Windows.Media.Brush"/> to <see cref="System.Drawing.Color"/>
    /// </summary>
    /// <param name="brush">Brush to convert</param>
    /// <returns>Color of type <see cref="System.Drawing.Color"/></returns>
    public static System.Drawing.Color ConvertMediaBrushToDrawingColor(Brush brush)
    {
      var mediaColor = ((SolidColorBrush) brush).Color;
      return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
    }

    /// <summary>
    /// Create popup window
    /// </summary>
    /// <param name="alert">Alert title</param>
    /// <param name="detail">Alert message</param>
    public static void CreatePopUpWindow(string alert, string detail)
    {
      var alertPopUp = new FancyNotificationPopUp
      {
        PopUpAlert = alert,
        PopUpAlertDetail = detail
      };
      Instance.CurrentEventManager.SendMessage(new ShowNotificationPopUpMessage(alertPopUp));
    }

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

    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    public async Task StartCalculationAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token) =>
      await _calculateHashsumController.StartCalculationAsync(collection, token).ConfigureAwait(false);

    /// <summary>
    /// Save hash
    /// </summary>
    /// <param name="collection"><see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    public async Task<bool> SaveHashAsync(ObservableCollection<WinMdChecksumData> collection, CancellationToken token) =>
      await _saveHashToFileController.SaveHashAsync(collection, token).ConfigureAwait(false);
  }
}
