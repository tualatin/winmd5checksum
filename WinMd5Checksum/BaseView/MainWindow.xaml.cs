using System;
using System.ComponentModel;
using System.Windows;
using log4net;
using Org.Vs.WinMd5.Core.Utils;
using Org.Vs.WinMd5.Data.Messages;


namespace Org.Vs.WinMd5.BaseView
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    private static readonly ILog LOG = LogManager.GetLogger(typeof(MainWindow));

    private bool _shouldClose;

    /// <summary>
    /// Standard constructor
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      EnvironmentContainer.Instance.CurrentEventManager.RegisterHandler<ShowNotificationPopUpMessage>(PopUpVisibilityChanged);

      Closing += OnMainWindowClosing;
    }

    protected override void OnStateChanged(EventArgs e)
    {
      if ( WindowState == WindowState.Minimized && EnvironmentContainer.Instance.CurrentSettings.MinimizeToTray )
        Hide();

      base.OnStateChanged(e);
    }

    private void OnMainWindowClosing(object sender, CancelEventArgs e)
    {
      if ( EnvironmentContainer.Instance.CurrentSettings.CloseToTray && !_shouldClose )
      {
        WindowState = WindowState.Minimized;

        Hide();
        e.Cancel = true;
        return;
      }

      Md5ChecksumDataGrid.SaveDataGridOptions();
    }

    private void PopUpVisibilityChanged(ShowNotificationPopUpMessage args)
    {
      if ( args == null )
        return;

      try
      {
        TbIcon.ShowCustomBalloon(args.Balloon, args.Animation, args.Timeout);
      }
      catch ( Exception ex )
      {
        LOG.Error(ex, "{0} caused a(n) {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.GetType().Name);
      }
    }

    private void OnTaskbarIconLeftMouseDown(object sender, RoutedEventArgs e)
    {
      if ( WindowState != WindowState.Minimized )
        return;

      Show();

      WindowState = WindowState.Normal;

      Activate();
      Focus();
    }

    private void OnContextMenuItemOpenClick(object sender, RoutedEventArgs e) => OnTaskbarIconLeftMouseDown(this, new RoutedEventArgs());

    private void OnContextMenuItemExitClick(object sender, RoutedEventArgs e)
    {
      _shouldClose = true;
      Close();
    }
  }
}
