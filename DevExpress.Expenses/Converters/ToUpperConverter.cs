using System;
using System.Globalization;
using System.Windows.Data;

namespace Expenses.Wpf {
    public class ToUpperConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null)
                return value;
            return value.ToString().ToUpper();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
