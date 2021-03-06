﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Org.Vs.WinMd5.UI.Converters
{
  /// <summary>
  /// Level to Visibility converter
  /// </summary>
  [ValueConversion(typeof(int), typeof(Visibility))]
  public class LevelToVisibilityConverter : IValueConverter
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
        return Visibility.Collapsed;

      if ( parameter is string )
        return i == 0 ? Visibility.Visible : Visibility.Collapsed;

      return i == 0 ? Visibility.Collapsed : Visibility.Visible;
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
