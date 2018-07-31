using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Org.Vs.WinMd5.Core.Data;
using Org.Vs.WinMd5.Core.Extensions;
using Org.Vs.WinMd5.Core.Interfaces;
using Org.Vs.WinMd5.Core.Utils;


namespace Org.Vs.WinMd5.Core.Controllers
{
  /// <summary>
  /// Settings controller
  /// </summary>
  public class SettingsController : ISettingsController
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(SettingsController));

    private static readonly object MyLock = new object();

    #region Properties

    /// <summary>
    /// Current environment settings
    /// </summary>
    public EnvironmentSettingsData CurrentSettings
    {
      get;
    } = new EnvironmentSettingsData();

    #endregion

    /// <summary>
    /// Reads current settings
    /// </summary>
    /// <returns>Task</returns>
    public async Task ReadSettingsAsync() =>
      await Task.Run(() => ReadSettings(), new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).ConfigureAwait(false);

    /// <summary>
    /// Writes current settings
    /// </summary>
    /// <returns>Task</returns>
    public async Task SaveSettingsAsync() =>
      await Task.Run(() => SaveSettings(), new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).ConfigureAwait(false);

    private void ReadSettings()
    {
      lock ( MyLock )
      {
        try
        {
          LOG.Trace($"Read {EnvironmentContainer.ApplicationTitle} settings");

          CurrentSettings.AlwaysOnTop = GetBoolFromSetting("AlwaysOnTop");
          CurrentSettings.UpperCaseHash = GetBoolFromSetting("UpperCase");
          CurrentSettings.MinimizeToTray = GetBoolFromSetting("MinimizeToTray");
          CurrentSettings.CloseToTray = GetBoolFromSetting("CloseToTray");
          CurrentSettings.SingleInstance = GetBoolFromSetting("SingleInstance");

          CurrentSettings.Md5IsEnabled = GetBoolFromSetting("Md5Enable");
          CurrentSettings.Sha1IsEnabled = GetBoolFromSetting("Sha1Enable");
          CurrentSettings.Sha256IsEnabled = GetBoolFromSetting("Sha256Enable");
          CurrentSettings.Sha384IsEnabled = GetBoolFromSetting("Sha384Enable");
          CurrentSettings.Sha512IsEnabled = GetBoolFromSetting("Sha512Enable");

        }
        catch ( Exception ex )
        {
          LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
        }
      }
    }

    private void SaveSettings()
    {
      lock ( MyLock )
      {
        try
        {
          LOG.Trace($"Save {EnvironmentContainer.ApplicationTitle} settings");

          Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

          if ( config.AppSettings.Settings.Count <= 0 )
            return;

          WriteValueToSetting(config, "AlwaysOnTop", CurrentSettings.AlwaysOnTop);
          WriteValueToSetting(config, "UpperCase", CurrentSettings.UpperCaseHash);
          WriteValueToSetting(config, "MinimizeToTray", CurrentSettings.MinimizeToTray);
          WriteValueToSetting(config, "CloseToTray", CurrentSettings.CloseToTray);
          WriteValueToSetting(config, "SingleInstance", CurrentSettings.SingleInstance);

          WriteValueToSetting(config, "Md5Enable", CurrentSettings.Md5IsEnabled);
          WriteValueToSetting(config, "Sha1Enable", CurrentSettings.Sha1IsEnabled);
          WriteValueToSetting(config, "Sha256Enable", CurrentSettings.Sha256IsEnabled);
          WriteValueToSetting(config, "Sha384Enable", CurrentSettings.Sha384IsEnabled);
          WriteValueToSetting(config, "Sha512Enable", CurrentSettings.Sha512IsEnabled);

          config.Save(ConfigurationSaveMode.Modified);
          ConfigurationManager.RefreshSection("appSettings");
        }
        catch ( Exception ex )
        {
          LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
        }
      }
    }

    private static bool GetBoolFromSetting(string setting, bool defaultValue = false) => ConfigurationManager.AppSettings[setting].ConvertToBool(defaultValue);

    private static void WriteValueToSetting(Configuration config, string setting, object value)
    {
      Arg.NotNull(config, "Configuration");
      config.AppSettings.Settings[setting].Value = value.ToString();
    }
  }
}
