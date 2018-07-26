using System.Threading.Tasks;
using Org.Vs.WinMd5.Core.Data;


namespace Org.Vs.WinMd5.Core.Interfaces
{
  /// <summary>
  /// Settings controller interface
  /// </summary>
  public interface ISettingsController
  {
    #region Properties

    /// <summary>
    /// Current environment settings
    /// </summary>
    EnvironmentSettingsData CurrentSettings
    {
      get;
    }

    #endregion

    /// <summary>
    /// Reads current settings
    /// </summary>
    /// <returns>Task</returns>
    Task ReadSettingsAsync();

    /// <summary>
    /// Writes current settings
    /// </summary>
    /// <returns>Task</returns>
    Task SaveSettingsAsync();
  }
}
