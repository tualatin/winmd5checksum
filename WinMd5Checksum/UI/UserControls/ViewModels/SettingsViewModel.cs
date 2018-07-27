using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;
using Org.Vs.WinMd5.Core.Data;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.UI.UserControls.Interfaces;


namespace Org.Vs.WinMd5.UI.UserControls.ViewModels
{
  /// <summary>
  /// Settings view model
  /// </summary>
  public class SettingsViewModel : ISettingsViewModel
  {
    private EnvironmentSettingsData.MementoEnvironmentSettings _mementoSettings;

    /// <summary>
    /// Standard constructor
    /// </summary>
    public SettingsViewModel() => _mementoSettings = EnvironmentContainer.Instance.CurrentSettings.SaveToMemento();

    #region Commands

    private IAsyncCommand _saveCommand;

    /// <summary>
    /// Save command
    /// </summary>
    public IAsyncCommand SaveCommand => _saveCommand ?? (_saveCommand = AsyncCommand.Create(ExecuteSaveCommandAsync));

    private ICommand _closeOptionsCommand;

    /// <summary>
    /// Close options dialog command
    /// </summary>
    public ICommand CloseOptionsCommand => _closeOptionsCommand ?? (_closeOptionsCommand = new RelayCommand(p => ExecuteCloseOptionsCommand((Window) p)));

    #endregion

    #region Command functions

    private void ExecuteCloseOptionsCommand(Window window)
    {
      MouseService.SetBusyState();

      if ( _mementoSettings != null )
        EnvironmentContainer.Instance.CurrentSettings.RestoreFromMemento(_mementoSettings);

      _mementoSettings = null;
      window?.Close();
    }

    private async Task ExecuteSaveCommandAsync()
    {
      MouseService.SetBusyState();

      _mementoSettings = null;
      await EnvironmentContainer.Instance.WriteSettingsAsync().ConfigureAwait(false);
    }

    #endregion
  }
}
