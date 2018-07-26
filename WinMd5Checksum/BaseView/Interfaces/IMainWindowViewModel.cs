using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.BaseView.Interfaces
{
  public interface IMainWindowViewModel
  {

    /// <summary>
    /// <see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/>
    /// </summary>
    ObservableCollection<WinMdChecksumData> MdChecksumCollection
    {
      get;
    }

    /// <summary>
    /// <see cref="ListCollectionView"/> of <see cref="WinMdChecksumData"/>
    /// </summary>
    ListCollectionView CollectionView
    {
      get;
      set;
    }

    /// <summary>
    /// Loaded command
    /// </summary>
    IAsyncCommand LoadedCommand
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
  }
}
