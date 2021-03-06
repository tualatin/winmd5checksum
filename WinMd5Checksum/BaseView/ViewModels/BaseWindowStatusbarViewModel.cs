﻿using Org.Vs.WinMd5.BaseView.Interfaces;
using Org.Vs.WinMd5.Core.Data.Base;


namespace Org.Vs.WinMd5.BaseView.ViewModels
{
  /// <summary>
  /// BaseWindowStatusbar view model
  /// </summary>
  public class BaseWindowStatusbarViewModel : NotifyMaster, IBaseWindowStatusbarViewModel
  {
    private static BaseWindowStatusbarViewModel instance;

    /// <summary>
    /// Current instance
    /// </summary>
    public static BaseWindowStatusbarViewModel Instance => instance ?? (instance = new BaseWindowStatusbarViewModel());

    private BaseWindowStatusbarViewModel()
    {
    }

    #region Statusbar properties

    private string _currentStatusBarBackgroundColorHex;

    /// <summary>
    /// CurrentStatusBarBackground color as string
    /// </summary>
    public string CurrentStatusBarBackgroundColorHex
    {
      get => _currentStatusBarBackgroundColorHex;
      set
      {
        if ( Equals(value, _currentStatusBarBackgroundColorHex) )
          return;

        _currentStatusBarBackgroundColorHex = value;
        OnPropertyChanged(nameof(CurrentStatusBarBackgroundColorHex));
      }
    }

    private string _currentBusyState;

    /// <summary>
    /// CurrentBusy state
    /// </summary>
    public string CurrentBusyState
    {
      get => _currentBusyState;
      set
      {
        if ( Equals(value, _currentBusyState) )
          return;

        _currentBusyState = value;
        OnPropertyChanged(nameof(CurrentBusyState));
      }
    }

    private string _elapsedTime;

    /// <summary>
    /// Elapsed time
    /// </summary>
    public string ElapsedTime
    {
      get => _elapsedTime;
      set
      {
        if ( Equals(value, _elapsedTime) )
          return;

        _elapsedTime = value;
        OnPropertyChanged();
      }
    }

    #endregion
  }
}
