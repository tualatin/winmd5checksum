using System.Windows.Data;
using System.Drawing;
using System.Windows;
using WinMd5Checksum.Data;


namespace WinMd5Checksum.Utils
{
  public class MyValueConverter: IValueConverter
  {
    #region IValueConverter Members

    public object Convert (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      Md5Structure input = value as Md5Structure;

      if (input != null)
      {
        switch (input.result)
        {
        case "FAILED":

          return (Brushes.Red);

        default:

          return (DependencyProperty.UnsetValue);
        }
      }
      return (Brushes.White);
    }

    public object ConvertBack (object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new System.NotImplementedException ( );
    }

    #endregion
  }
}
