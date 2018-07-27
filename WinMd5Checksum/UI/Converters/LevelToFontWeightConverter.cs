using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Org.Vs.WinMd5.UI.Converters
{
  /// <summary>
  /// Level to FontWeight converter
  /// </summary>
  [ValueConversion(typeof(int), typeof(FontWeights))]
  public class LevelToFontWeightConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ( !(value is int i) )
        return FontWeights.Normal;

      return i == 0 ? FontWeights.Bold : FontWeights.Normal;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
