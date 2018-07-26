using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;


namespace Org.Vs.WinMd5.UI.Converters
{
  /// <summary>
  /// Level to FileName converter
  /// </summary>
  [ValueConversion(typeof(int), typeof(string))]
  public class LevelToFileNameConverter : IValueConverter
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
      if ( !(value is string text) )
        return string.Empty;

      try
      {
        return Path.GetFileName(text);
      }
      catch
      {
        return value;
      }
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
