using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Org.Vs.WinMd5.BaseView.Interfaces;
using Org.Vs.WinMd5.Controllers.Commands;
using Org.Vs.WinMd5.Core.Data.Base;
using Org.Vs.WinMd5.Core.Enums;
using Org.Vs.WinMd5.Core.Utils;


namespace Org.Vs.WinMd5.BaseView.ViewModels
{
  /// <summary>
  /// MainWindow view model
  /// </summary>
  public class MainWindowViewModel : NotifyMaster, IMainWindowViewModel
  {
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

    public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(p => ExecuteLoadedCommand()));

    #endregion

    #region Command functions

    private void ExecuteLoadedCommand()
    {
      SetCurrentBusinessData();
    }

    #endregion

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
