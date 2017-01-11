using Org.Vs.WinMd5Checksum.Data;
using Org.Vs.WinMd5Checksum.Utils;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;


namespace Org.Vs.WinMd5Checksum
{
  /// <summary>
  /// Interaction logic for About.xaml
  /// </summary>
  public partial class About : Window
  {
    public About ()
    {
      InitializeComponent();

      Title = "About " + LogFile.ApplicationCaption();

      Assembly assembly = Assembly.GetExecutingAssembly();
      labelBuildDate.Content = (BuildDate.GetBuildDateTime(assembly)).ToString("dd.MM.yyyy HH:mm:ss");
      labelVersion.Content = assembly.GetName().Version;

      PreviewKeyDown += HandleEsc;
    }

    private void Hyperlink_RequestNavigate (object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }

    private void btnOk_Click (object sender, RoutedEventArgs e)
    {
      Close();
    }

    public void HandleEsc (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
        btnOk_Click(sender, e);
    }
  }
}
