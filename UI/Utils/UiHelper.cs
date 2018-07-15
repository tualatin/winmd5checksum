using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Org.Vs.WinMd5Checksum.Core.Utils;
using Org.Vs.WinMd5Checksum.Data.Messages;
using Org.Vs.WinMd5Checksum.UI.Extensions;
using Org.Vs.WinMd5Checksum.UI.UserControls;


namespace Org.Vs.WinMd5Checksum.UI.Utils
{
  public static class UiHelper
  {
    /// <summary>
    /// Create popup window
    /// </summary>
    /// <param name="alert">Alert title</param>
    /// <param name="detail">Alert message</param>
    public static void CreatePopUpWindow(string alert, string detail)
    {
      var alertPopUp = new FancyNotificationPopUp
      {
        PopUpAlert = alert,
        PopUpAlertDetail = detail
      };
      EnvironmentContainer.Instance.CurrentEventManager.SendMessage(new ShowNotificationPopUpMessage(alertPopUp));
    }

    /// <summary>
    /// Get horizontal scrollbar grid
    /// </summary>
    /// <param name="scrollViewer"><see cref="DependencyObject"/></param>
    /// <returns><see cref="Grid"/> horizontal scrollbar grid</returns>
    public static Grid GetHorizontalScrollBarGrid(DependencyObject scrollViewer)
    {
      if ( scrollViewer == null )
        return null;

      var scrollBars = scrollViewer.Descendents().OfType<ScrollBar>().Where(p => p.Visibility == Visibility.Visible);

      foreach ( var scrollBar in scrollBars )
      {
        var grid = scrollBar.Descendents().OfType<Grid>().FirstOrDefault(p => p.Name == "GridHorizontalScrollBar");

        if ( grid == null )
          continue;

        return grid;
      }
      return null;
    }
  }
}
