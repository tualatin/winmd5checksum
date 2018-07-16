using System.Windows.Input;
using Org.Vs.WinMd5.Controllers.Interfaces;


namespace Org.Vs.WinMd5.UI.UserControls.Interfaces
{
  /// <summary>
  /// About view model interface
  /// </summary>
  public interface IAboutViewModel : IViewModelBase
  {
    #region Properties

    /// <summary>
    /// Current version
    /// </summary>
    string Version
    {
      get;
      set;
    }

    /// <summary>
    /// Build date
    /// </summary>
    string BuildDate
    {
      get;
      set;
    }

    /// <summary>
    /// Author
    /// </summary>
    string Author
    {
      get;
      set;
    }

    /// <summary>
    /// Uptime
    /// </summary>
    string UpTime
    {
      get;
      set;
    }

    #endregion

    #region Commands

    /// <summary>
    /// Request navigate command
    /// </summary>
    ICommand RequestNavigateCommand
    {
      get;
    }

    /// <summary>
    /// Donate command
    /// </summary>
    ICommand DonateCommand
    {
      get;
    }

    #endregion
  }
}
