using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using log4net;
using Org.Vs.WinMd5.BaseView.Interfaces;
using Org.Vs.WinMd5.Controllers.Commands;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Enums;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.UI.UserControls;


namespace Org.Vs.WinMd5.BaseView.ViewModels
{
  /// <summary>
  /// MainWindow view model
  /// </summary>
  public class MainWindowViewModel : NotifyMaster, IMainWindowViewModel
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(MainWindowViewModel));

    private EStatusbarState _currentStatusbarState;
    private readonly IBaseWindowStatusbarViewModel _baseWindowStatusbarViewModel;


    /// <summary>
    /// Standard constructor
    /// </summary>
    public MainWindowViewModel()
    {
      // Set XAML language culture info
      FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
        new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.Name)));

      _currentStatusbarState = EStatusbarState.Default;
      _baseWindowStatusbarViewModel = BaseWindowStatusbarViewModel.Instance;
    }

    #region Commands

    private ICommand _loadedCommand;

    /// <summary>
    /// Loaded command
    /// </summary>
    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(p => ExecuteLoadedCommand()));

    private IAsyncCommand _wndClosingCommand;

    /// <summary>
    /// Window closing command
    /// </summary>
    public IAsyncCommand WndClosingCommand => _wndClosingCommand ?? (_wndClosingCommand = AsyncCommand.Create((p, t) => ExecuteWndClosingCommandAsync(p)));

    private ICommand _aboutCommand;

    /// <summary>
    /// About command
    /// </summary>
    public ICommand AboutCommand => _aboutCommand ?? (_aboutCommand = new RelayCommand(p => ExecuteOpenAboutWindow((Window) p)));

    private ICommand _hintCommand;

    /// <summary>
    /// Hint command
    /// </summary>
    public ICommand HintCommand => _hintCommand ?? (_hintCommand = new RelayCommand(p => ExecuteOpenHintWindow((Window) p)));

    #endregion

    #region Command functions

    private void ExecuteOpenHintWindow(Window window)
    {
      if ( window == null )
        return;

      var hint = new HintWindow
      {
        Owner = window
      };
      hint.ShowDialog();
    }

    private void ExecuteOpenAboutWindow(Window window)
    {
      if ( window == null )
        return;

      var about = new AboutWindow
      {
        Owner = window
      };
      about.ShowDialog();
    }

    private void ExecuteLoadedCommand()
    {
      LOG.Trace($"{EnvironmentContainer.ApplicationTitle} startup completed!");

      SetCurrentBusinessData();
    }

    private async Task ExecuteWndClosingCommandAsync(object param)
    {
      if ( !(param is CancelEventArgs e) )
        return;

      if ( e.Cancel )
        return;

      await DeleteLogFilesAsync().ConfigureAwait(false);
      LOG.Trace($"{EnvironmentContainer.ApplicationTitle} closing, goodbye!");
    }

    #endregion

    private async Task DeleteLogFilesAsync()
    {
      LOG.Trace("Delete log files older than 5 days...");

      if ( !Directory.Exists("logs") )
        return;

      await Task.Run(
        () =>
        {
          try
          {
            var files = new DirectoryInfo("logs").GetFiles("*.log");

            Parallel.ForEach(files.Where(p => DateTime.Now - p.LastWriteTimeUtc > TimeSpan.FromDays(5)), item =>
            {
              try
              {
                item.Delete();
              }
              catch ( Exception ex )
              {
                LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
              }
            });
          }
          catch ( Exception ex )
          {
            LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
          }
        }, new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token).ConfigureAwait(false);
    }

    private void SetCurrentBusinessData()
    {
      switch ( _currentStatusbarState )
      {
      case EStatusbarState.Busy:

        _baseWindowStatusbarViewModel.CurrentStatusBarBackgroundColorHex = EnvironmentContainer.StatusBarBusyBackgroundColor;
        _baseWindowStatusbarViewModel.CurrentBusyState = Application.Current.TryFindResource("Record").ToString();
        break;

      case EStatusbarState.Default:

        _baseWindowStatusbarViewModel.CurrentStatusBarBackgroundColorHex = EnvironmentContainer.StatusBarInactiveBackgroundColor;
        _baseWindowStatusbarViewModel.CurrentBusyState = Application.Current.TryFindResource("TrayIconReady").ToString();
        break;

      default:

        throw new NotImplementedException();
      }
    }
  }
}
