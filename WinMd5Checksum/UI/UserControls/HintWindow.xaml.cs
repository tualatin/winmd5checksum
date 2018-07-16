using System.Windows.Input;


namespace Org.Vs.WinMd5.UI.UserControls
{
  /// <summary>
  /// Interaction logic for HintWindow.xaml
  /// </summary>
  public partial class HintWindow
  {
    /// <summary>
    /// Standarc constructor
    /// </summary>
    public HintWindow()
    {
      InitializeComponent();
      PreviewKeyDown += HandleEsc;
    }

    private void HandleEsc(object sender, KeyEventArgs e)
    {
      if ( e.Key != Key.Escape )
        return;

      e.Handled = true;
      Close();
    }
  }
}
