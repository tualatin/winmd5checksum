using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace Org.Vs.WinMd5.UI.Converters.MultiConverters
{
  /// <summary>
  /// DataGrid background MultiConverter
  /// </summary>
  public class DataGridBackgroundMultiConverter : IMultiValueConverter
  {
    private readonly string _failed;

    /// <summary>
    /// Standard constructor
    /// </summary>
    public DataGridBackgroundMultiConverter() => _failed = Application.Current.TryFindResource("HashCompareFailed").ToString();

    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="values">Value</param>
    /// <param name="targetType">Target type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture</param>
    /// <returns>Converted value</returns>
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      Brush result = Brushes.White;

      foreach ( object value in values )
      {
        if ( !(value is string myResult) )
          continue;

        if ( string.Compare(myResult, _failed, StringComparison.InvariantCultureIgnoreCase) != 0 )
          continue;

        result = Brushes.LightPink;
        break;
      }
      return result;
    }

    /// <summary>
    /// Convert back
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="targetTypes">TargetType</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Current culture</param>
    /// <returns>Back converted value</returns>
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
