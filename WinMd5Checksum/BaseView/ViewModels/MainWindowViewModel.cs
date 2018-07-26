﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using log4net;
using Org.Vs.WinMd5.BaseView.Interfaces;
using Org.Vs.WinMd5.Controllers.Commands;
using Org.Vs.WinMd5.Controllers.Commands.Interfaces;
using Org.Vs.WinMd5.Controllers.Interfaces;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Enums;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.Data;
using Org.Vs.WinMd5.UI.Extensions;
using Org.Vs.WinMd5.UI.UserControls;


namespace Org.Vs.WinMd5.BaseView.ViewModels
{
  /// <summary>
  /// MainWindow view model
  /// </summary>
  public class MainWindowViewModel : NotifyMaster, IMainWindowViewModel, IFileDragDropTarget
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(MainWindowViewModel));

    private EStatusbarState _currentStatusbarState;
    private readonly IBaseWindowStatusbarViewModel _baseWindowStatusbarViewModel;
    private readonly Stopwatch _myStopwatch;
    private readonly string _elapsedTimeResource;
    private CancellationTokenSource _cts;

    #region Properties

    /// <summary>
    /// <see cref="ObservableCollection{T}"/> of <see cref="WinMdChecksumData"/>
    /// </summary>
    public ObservableCollection<WinMdChecksumData> MdChecksumCollection
    {
      get;
    }

    /// <summary>
    /// <see cref="ListCollectionView"/> of <see cref="WinMdChecksumData"/>
    /// </summary>
    public ListCollectionView CollectionView
    {
      get;
      set;
    }

    #endregion

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
      _myStopwatch = new Stopwatch();
      _elapsedTimeResource = Application.Current.TryFindResource("ElapsedTime").ToString();

      MdChecksumCollection = new ObservableCollection<WinMdChecksumData>();
      CollectionView = (ListCollectionView) new CollectionViewSource { Source = MdChecksumCollection }.View;
      HashsumCollectionViewHolder.Cv = CollectionView;

      ((AsyncCommand<object>) LoadedCommand).PropertyChanged += OnLoadedPropertyChanged;
      ((AsyncCommand<object>) StartCalculationCommand).PropertyChanged += OnStartCalculationPropertyChanged;
    }

    #region Commands

    private IAsyncCommand _loadedCommand;

    /// <summary>
    /// Loaded command
    /// </summary>
    public IAsyncCommand LoadedCommand => _loadedCommand ?? (_loadedCommand = AsyncCommand.Create(ExecuteLoadedCommandAsync));

    private IAsyncCommand _wndClosingCommand;

    /// <summary>
    /// Window closing command
    /// </summary>
    public IAsyncCommand WndClosingCommand => _wndClosingCommand ?? (_wndClosingCommand = AsyncCommand.Create((p, t) => ExecuteWndClosingCommandAsync(p)));

    private ICommand _aboutCommand;

    /// <summary>
    /// About command
    /// </summary>
    public ICommand AboutCommand => _aboutCommand ?? (_aboutCommand = new RelayCommand(ExecuteOpenAboutWindow));

    private ICommand _hintCommand;

    /// <summary>
    /// Hint command
    /// </summary>
    public ICommand HintCommand => _hintCommand ?? (_hintCommand = new RelayCommand(ExecuteOpenHintWindow));

    private ICommand _openFileCommand;

    /// <summary>
    /// Open file command
    /// </summary>
    public ICommand OpenFileCommand => _openFileCommand ?? (_openFileCommand = new RelayCommand(p => _currentStatusbarState != EStatusbarState.Busy, p => ExecuteOpenFileCommand()));

    private ICommand _previewDragEnterCommand;

    /// <summary>
    /// Preview drag enter command
    /// </summary>
    public ICommand PreviewDragEnterCommand => _previewDragEnterCommand ?? (_previewDragEnterCommand = new RelayCommand(ExecutePreviewDragEnterCommand));

    private ICommand _clearAllCommand;

    /// <summary>
    /// Clear all command
    /// </summary>
    public ICommand ClearAllCommand => _clearAllCommand ?? (_clearAllCommand = new RelayCommand(p => CanExecuteClearAllCommand(), p => ExecuteClearAllCommand()));

    private IAsyncCommand _startCalculationCommand;

    /// <summary>
    /// Start calculation command
    /// </summary>
    public IAsyncCommand StartCalculationCommand => _startCalculationCommand ?? (_startCalculationCommand = AsyncCommand.Create(p => CanExecuteStartCalculation(),
                                                      ExecuteStartCalculationCommandAsync));

    private ICommand _stopCommand;

    /// <summary>
    /// Stop command
    /// </summary>
    public ICommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(p => _currentStatusbarState == EStatusbarState.Busy, p => ExecuteStopCommand()));

    private IAsyncCommand _saveCommand;

    /// <summary>
    /// Save command
    /// </summary>
    public IAsyncCommand SaveCommand => _saveCommand ?? (_saveCommand = AsyncCommand.Create(p => CanExecuteSaveCommand(), ExecuteSaveCommandAsync));

    private ICommand _toggleAlwaysOnTopCommand;

    /// <summary>
    /// ToggleAlwaysOnTop command
    /// </summary>
    public ICommand ToggleAlwaysOnTopCommand => _toggleAlwaysOnTopCommand ?? (_toggleAlwaysOnTopCommand = new RelayCommand(p => ExecuteToggleAlwaysOnTopCommand()));

    #endregion

    #region Command functions

    private void ExecuteToggleAlwaysOnTopCommand() => EnvironmentContainer.Instance.CurrentSettings.AlwaysOnTop = !EnvironmentContainer.Instance.CurrentSettings.AlwaysOnTop;

    private bool CanExecuteSaveCommand()
    {
      if ( MdChecksumCollection == null || MdChecksumCollection.Count == 0 )
        return false;

      if ( MdChecksumCollection.Any(p => string.IsNullOrWhiteSpace(p.Md5Hash) && string.IsNullOrWhiteSpace(p.Sha1Hash) && string.IsNullOrWhiteSpace(p.Sha256Hash) &&
                                         string.IsNullOrWhiteSpace(p.Sha512Hash)) )
        return false;

      return _currentStatusbarState != EStatusbarState.Busy;
    }

    private async Task ExecuteSaveCommandAsync()
    {
      if ( InteractionService.ShowQuestionMessageBox(Application.Current.TryFindResource("QWantToSave").ToString()) == MessageBoxResult.No )
        return;

      MouseService.SetBusyState();

      _cts?.Dispose();
      _cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

      bool result = await EnvironmentContainer.Instance.SaveHashAsync(MdChecksumCollection, _cts.Token).ConfigureAwait(false);

      if ( result )
        InteractionService.ShowInformationMessageBox(Application.Current.TryFindResource("SuccessfullySaved").ToString());
      else
        InteractionService.ShowErrorMessageBox(Application.Current.TryFindResource("SaveFailed").ToString());
    }

    private void ExecuteStopCommand()
    {
      MouseService.SetBusyState();

      _cts?.Cancel();
      _myStopwatch.Reset();
      _currentStatusbarState = EStatusbarState.Default;
      SetCurrentBusinessData();
      EnvironmentContainer.CreatePopUpWindow(EnvironmentContainer.ApplicationTitle, Application.Current.TryFindResource("CalculationStopped").ToString());
    }

    private bool CanExecuteStartCalculation() => MdChecksumCollection != null && MdChecksumCollection.Count > 0 && _currentStatusbarState != EStatusbarState.Busy;

    private async Task ExecuteStartCalculationCommandAsync()
    {
      MouseService.SetBusyState();
      EnvironmentContainer.CreatePopUpWindow(EnvironmentContainer.ApplicationTitle, Application.Current.TryFindResource("CalculationStart").ToString());

      _myStopwatch.Reset();
      _myStopwatch.Start();

      _currentStatusbarState = EStatusbarState.Busy;
      SetCurrentBusinessData();

      _cts?.Dispose();
      _cts = new CancellationTokenSource();

      await EnvironmentContainer.Instance.StartCalculationAsync(MdChecksumCollection, _cts.Token).ConfigureAwait(false);
    }

    private bool CanExecuteClearAllCommand() => MdChecksumCollection != null && MdChecksumCollection.Count > 0 && _currentStatusbarState != EStatusbarState.Busy;

    private void ExecuteClearAllCommand()
    {
      MouseService.SetBusyState();
      MdChecksumCollection.Clear();
      _myStopwatch.Reset();

      SetCurrentBusinessData();
      OnPropertyChanged(nameof(MdChecksumCollection));
      OnPropertyChanged(nameof(CollectionView));
    }

    private void ExecutePreviewDragEnterCommand(object parameter)
    {
      if ( !(parameter is DragEventArgs e) )
        return;

      e.Handled = true;
      e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;
    }

    private void ExecuteOpenFileCommand()
    {
      bool result = InteractionService.OpenFileDialog(out string fileName, "All files(*.*)|*.*", EnvironmentContainer.ApplicationTitle);

      if ( result != true )
        return;

      if ( string.IsNullOrWhiteSpace(fileName) )
        return;

      MdChecksumCollection.Add(new WinMdChecksumData
      {
        FileName = fileName
      });

      OnPropertyChanged(nameof(MdChecksumCollection));
      OnPropertyChanged(nameof(CollectionView));
    }

    private void ExecuteOpenHintWindow(object arg)
    {
      if ( !(arg is Button button) )
        return;

      MainWindow mainWindow = GetMainWindow(button);

      if ( mainWindow == null )
        return;

      var hint = new HintWindow
      {
        Owner = mainWindow
      };
      hint.ShowDialog();
    }

    private void ExecuteOpenAboutWindow(object arg)
    {
      if ( !(arg is Button button) )
        return;

      MainWindow mainWindow = GetMainWindow(button);

      if ( mainWindow == null )
        return;

      var about = new AboutWindow
      {
        Owner = mainWindow
      };
      about.ShowDialog();
    }

    private async Task ExecuteLoadedCommandAsync()
    {
      await EnvironmentContainer.Instance.ReadSettingsAsync();
      await CreateUserSettingFolderAsync();
    }

    private async Task ExecuteWndClosingCommandAsync(object param)
    {
      if ( !(param is CancelEventArgs e) )
        return;

      if ( e.Cancel )
        return;

      await EnvironmentContainer.Instance.WriteSettingsAsync();
      await DeleteLogFilesAsync();

      LOG.Trace($"{EnvironmentContainer.ApplicationTitle} closing, goodbye!");
    }

    #endregion

    private void OnLoadedPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if ( !e.PropertyName.Equals("IsSuccessfullyCompleted") )
        return;

      SetCurrentBusinessData();
      LOG.Trace($"{EnvironmentContainer.ApplicationTitle} startup completed!");
    }

    private void OnStartCalculationPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if ( !e.PropertyName.Equals("IsSuccessfullyCompleted") )
        return;

      _currentStatusbarState = EStatusbarState.Default;
      _myStopwatch?.Stop();
      SetCurrentBusinessData();
      EnvironmentContainer.CreatePopUpWindow(EnvironmentContainer.ApplicationTitle, Application.Current.TryFindResource("CalculationFinished").ToString());

      OnPropertyChanged(nameof(MdChecksumCollection));
      OnPropertyChanged(nameof(CollectionView));
    }

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

        _baseWindowStatusbarViewModel.ElapsedTime = string.Empty;
        _baseWindowStatusbarViewModel.CurrentStatusBarBackgroundColorHex = EnvironmentContainer.StatusBarBusyBackgroundColor;
        _baseWindowStatusbarViewModel.CurrentBusyState = Application.Current.TryFindResource("Record").ToString();
        break;

      case EStatusbarState.Default:

        if ( _myStopwatch.ElapsedMilliseconds > 0 && !string.IsNullOrWhiteSpace(_elapsedTimeResource) )
          _baseWindowStatusbarViewModel.ElapsedTime = string.Format(_elapsedTimeResource, _myStopwatch.Elapsed);
        else
          _baseWindowStatusbarViewModel.ElapsedTime = string.Empty;

        _baseWindowStatusbarViewModel.CurrentStatusBarBackgroundColorHex = EnvironmentContainer.StatusBarInactiveBackgroundColor;
        _baseWindowStatusbarViewModel.CurrentBusyState = Application.Current.TryFindResource("TrayIconReady").ToString();
        break;

      default:

        throw new NotImplementedException();
      }
    }

    private async Task CreateUserSettingFolderAsync()
    {
      await Task.Run(() =>
      {
        try
        {
          if ( Directory.Exists(EnvironmentContainer.UserSettingsPath) )
            return;

          LOG.Info("Create user settings folder");
          Directory.CreateDirectory(EnvironmentContainer.UserSettingsPath);
        }
        catch ( Exception ex )
        {
          LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
        }
      }).ConfigureAwait(false);
    }

    private MainWindow GetMainWindow(DependencyObject button) => button.Ancestors().OfType<MainWindow>().FirstOrDefault();

    public void OnFileDrop(string[] filePaths)
    {
      if ( filePaths == null || filePaths.Length == 0 )
        return;

      foreach ( string file in filePaths )
      {
        MdChecksumCollection.Add(new WinMdChecksumData
        {
          FileName = file
        });
      }

      OnPropertyChanged(nameof(MdChecksumCollection));
      OnPropertyChanged(nameof(CollectionView));
    }
  }
}
