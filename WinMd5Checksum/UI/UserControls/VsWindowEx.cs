using System.Windows;
using Org.Vs.WinMd5.UI.Extensions;


namespace Org.Vs.WinMd5.UI.UserControls
{
  /// <summary>
  /// WindowEx class
  /// </summary>
  public class VsWindowEx : Window
  {
    /// <summary>
    /// Standard constructor
    /// </summary>
    // ReSharper disable once MemberCanBeProtected.Global
    public VsWindowEx()
    {
      SourceInitialized += (o, e) =>
      {
        this.HideMinimizeMaximizeButtons();
      };
    }

    /// <summary>
    /// Dialog can close
    /// </summary>
    public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof(CanClose), typeof(bool), typeof(VsWindowEx), new PropertyMetadata(false, CanCloseChanged));

    private static void CanCloseChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      if ( e.Property != CanCloseProperty || !(e.NewValue is bool canClose) )
        return;

      if ( canClose && sender is VsWindowEx wnd )
        wnd.Close();
    }

    /// <summary>
    /// Can close
    /// </summary>
    public bool CanClose
    {
      get => (bool) GetValue(CanCloseProperty);
      set => SetValue(CanCloseProperty, value);
    }
  }
}
