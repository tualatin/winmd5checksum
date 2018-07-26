using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Org.Vs.WinMd5.UI.Converters
{
  /// <summary>
  /// Level to Margin converter
  /// </summary>
  [ValueConversion(typeof(int), typeof(Thickness))]
  public class LevelToMarginConverter : IValueConverter
  {
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">Target type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Visibility value</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if ( !(value is int i) )
        return new Thickness(0, 0, 0, 0);

      return i == 0 ? new Thickness(0, 0, 0, 0) : new Thickness(i + 20, 0, 5, 0);
    }

    /// <summary>
    /// Convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">Target type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Back converted value</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
