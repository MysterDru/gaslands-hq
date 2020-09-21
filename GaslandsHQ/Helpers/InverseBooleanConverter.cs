using System;
using System.Globalization;
using Xamarin.Forms;

namespace GaslandsHQ.Helpers
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return !(bool)value;
            }
            catch { return false; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return !(bool)value;
            }
            catch { return true; }
        }
    }
}
