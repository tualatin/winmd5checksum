using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;


namespace Org.Vs.WinMd5.BaseView.Interfaces
{
  public interface IMainWindowViewModel
  {
    /// <summary>
    /// Loaded command
    /// </summary>
    ICommand LoadedCommand
    {
      get;
    }

    /// <summary>
    /// Window closing command
    /// </summary>
    IAsyncCommand WndClosingCommand
    {
      get;
    }
  }
}
