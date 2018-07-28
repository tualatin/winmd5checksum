using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Org.Vs.WinMd5.Controllers;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Core.Controllers;
using Org.Vs.WinMd5.Core.Data;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Interfaces;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.Data.Messages;
using Org.Vs.WinMd5.UI.UserControls;
using Org.Vs.WinMd5.UI.UserControls.DataModels;
using Org.Vs.WinMd5.UI.UserControls.DataModels.Data;


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
    private readonly ISettingsController _settingsController;

    private EnvironmentContainer()
    {
      UpTime = DateTime.Now;
      CurrentEventManager = new EventAggregator();
      _calculateHashsumController = new CalculateHash();
      _saveHashToFileController = new SaveHashToFile();
      _settingsController = new SettingsController();
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
    /// Application Homepage URL
    /// </summary>
    public static string ApplicationHomePageWebUrl => "https://github.com/tualatin/winmd5checksum";

    /// <summary>
    /// Application Issue URL
    /// </summary>
    public static string ApplicationIssueWebUrl => "https://github.com/tualatin/winmd5checksum/issues";

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

    /// <summary>
    /// Current up time
    /// </summary>
    public DateTime UpTime
    {
      get;
    }

    /// <summary>
    /// SettingsController
    /// </summary>
    public EnvironmentSettingsData CurrentSettings => _settingsController.CurrentSettings;

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
    /// Read current application settings
    /// </summary>
    /// <returns></returns>
    public async Task ReadSettingsAsync() => await _settingsController.ReadSettingsAsync().ConfigureAwait(false);

    /// <summary>
    /// Write current application settings
    /// </summary>
    /// <returns></returns>
    public async Task WriteSettingsAsync() => await _settingsController.SaveSettingsAsync().ConfigureAwait(false);

    /// <summary>
    /// Start hash calculation of data collection
    /// </summary>
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>Task</returns>
    public async Task StartCalculationAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token) =>
      await _calculateHashsumController.StartCalculationAsync(collection, token).ConfigureAwait(false);

    /// <summary>
    /// Save hash
    /// </summary>
    /// <param name="collection"><see cref="List{T}"/> of <see cref="VsDataGridHierarchialDataModel"/></param>
    /// <param name="token"><see cref="CancellationToken"/></param>
    /// <returns>If success <c>True</c> otherwise <c>False</c></returns>
    public async Task<bool> SaveHashAsync(List<VsDataGridHierarchialDataModel> collection, CancellationToken token) =>
      await _saveHashToFileController.SaveHashAsync(collection, token).ConfigureAwait(false);

    /// <summary>
    /// Create a <see cref="VsDataGridHierarchialData"/>
    /// </summary>
    /// <param name="dataManager"><see cref="VsDataGridHierarchialData"/></param>
    /// <param name="fileName">file name</param>
    /// <param name="enabled">Hash calculation is enabled</param>
    public void CreateHierachialDataObject(VsDataGridHierarchialData dataManager, string fileName, params bool[] enabled)
    {
      var fileContainer = new WinMdChecksumData
      {
        FileName = fileName
      };

      var model = new VsDataGridHierarchialDataModel
      {
        DataManager = dataManager,
        Data = fileContainer,
        IsVisible = true
      };

      model.AddChild(CreateChildModel(HashNames.Md5, enabled.Length == 0 ? CurrentSettings.Md5IsEnabled : enabled[0]));
      model.AddChild(CreateChildModel(HashNames.Sha1, enabled.Length == 0 ? CurrentSettings.Sha1IsEnabled : enabled[1]));
      model.AddChild(CreateChildModel(HashNames.Sha256, enabled.Length == 0 ? CurrentSettings.Sha256IsEnabled : enabled[2]));
      model.AddChild(CreateChildModel(HashNames.Sha384, enabled.Length == 0 ? CurrentSettings.Sha384IsEnabled : enabled[3]));
      model.AddChild(CreateChildModel(HashNames.Sha512, enabled.Length == 0 ? CurrentSettings.Sha512IsEnabled : enabled[4]));

      dataManager.RowData.Add(model);
      dataManager.Initialize();

      dataManager.First().IsExpanded = true;
    }

    private VsDataGridHierarchialDataModel CreateChildModel(string childName, bool isEnabled)
    {
      var child = new WinMdChecksumData
      {
        FileName = childName,
        HashIsEnabled = isEnabled
      };

      var childModel = new VsDataGridHierarchialDataModel
      {
        Data = child
      };

      return childModel;
    }
  }
}
