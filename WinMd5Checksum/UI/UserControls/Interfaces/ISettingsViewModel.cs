using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;


namespace Org.Vs.WinMd5.UI.UserControls.Interfaces
{
  /// <summary>
  /// Settings view model interface
  /// </summary>
  public interface ISettingsViewModel
  {
    /// <summary>
    /// Save command
    /// </summary>
    IAsyncCommand SaveCommand
    {
      get;
    }

    /// <summary>
    /// Close options dialog command
    /// </summary>
    ICommand CloseOptionsCommand
    {
      get;
    }
  }
}
