using System;
using System.Globalization;
using System.Windows.Data;
using Org.Vs.WinMd5.Data;


namespace Org.Vs.WinMd5.UI.Converters
{
  /// <summary>
  /// Row collection converter
  /// </summary>
  public class RowCollectionConverter : IValueConverter
  {
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">Target type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Converted value</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      int rn = 0;

      if ( HashsumCollectionViewHolder.Cv == null || value == null )
        return rn;

      rn = HashsumCollectionViewHolder.Cv.IndexOf(value);

      return rn + 1;
    }

    /// <summary>
    /// Convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetType">TargetType</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Current culture</param>
    /// <returns>Back converted value</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
