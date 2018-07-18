using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;


namespace Org.Vs.WinMd5.Controllers.Interfaces
{
  /// <summary>
  /// View model base interface
  /// </summary>
  public interface IViewModelBase
  {
    /// <summary>
    /// Loaded command
    /// </summary>
    IAsyncCommand LoadedCommand
    {
      get;
    }

    /// <summary>
    /// Unloaded command
    /// </summary>
    ICommand UnloadedCommand
    {
      get;
    }
  }
}
