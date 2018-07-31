using System.Windows.Data;
using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;
using Org.Vs.WinMd5.UI.UserControls.DataModels.Data;


namespace Org.Vs.WinMd5.BaseView.Interfaces
{
  public interface IMainWindowViewModel
  {
    /// <summary>
    /// <see cref="VsDataGridHierarchialData"/>
    /// </summary>
    VsDataGridHierarchialData HierarchialData
    {
      get;
    }

    /// <summary>
    /// <see cref="ListCollectionView"/>
    /// </summary>
    ListCollectionView CollectionView
    {
      get;
      set;
    }

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

    /// <summary>
    /// Settings command
    /// </summary>
    ICommand SettingsCommand
    {
      get;
    }

    /// <summary>
    /// About command
    /// </summary>
    ICommand AboutCommand
    {
      get;
    }

    /// <summary>
    /// Hint command
    /// </summary>
    ICommand HintCommand
    {
      get;
    }

    /// <summary>
    /// Open file command
    /// </summary>
    ICommand OpenFileCommand
    {
      get;
    }

    /// <summary>
    /// Preview drag enter command
    /// </summary>
    ICommand PreviewDragEnterCommand
    {
      get;
    }

    /// <summary>
    /// Clear all command
    /// </summary>
    ICommand ClearAllCommand
    {
      get;
    }

    /// <summary>
    /// Start calculation command
    /// </summary>
    IAsyncCommand StartCalculationCommand
    {
      get;
    }

    /// <summary>
    /// Stop command
    /// </summary>
    ICommand StopCommand
    {
      get;
    }

    /// <summary>
    /// Save command
    /// </summary>
    IAsyncCommand SaveCommand
    {
      get;
    }

    /// <summary>
    /// Exit command
    /// </summary>
    ICommand ExitCommand
    {
      get;
    }

    /// <summary>
    /// Open help command
    /// </summary>
    ICommand OpenHelpCommand
    {
      get;
    }
  }
}
