using System.Windows.Input;


namespace Org.Vs.WinMd5.UI.UserControls
{
  /// <summary>
  /// Interaction logic for AboutWindow.xaml
  /// </summary>
  public partial class AboutWindow
  {
    /// <summary>
    /// Standard constructor
    /// </summary>
    public AboutWindow()
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
