using System;
using System.ComponentModel;
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

    /// <summary>
    /// Standard constructor
    /// </summary>
    public MainWindow()
    {
      InitializeComponent();
      EnvironmentContainer.Instance.CurrentEventManager.RegisterHandler<ShowNotificationPopUpMessage>(PopUpVisibilityChanged);

      Closing += OnMainWindowClosing;
    }

    private void OnMainWindowClosing(object sender, CancelEventArgs e) => Md5ChecksumDataGrid.SaveDataGridOptions();

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
  }
}
