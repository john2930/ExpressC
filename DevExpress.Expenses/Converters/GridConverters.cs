using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Expenses.Wpf {
    public class ViewToVisibilityConverter : MarkupExtension, IValueConverter {
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value is SelectionView ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
    public class RowHeightToBrushConverter : MarkupExtension, IValueConverter {
        public Color BorderBrush { get; set; }
        public Color BackgroundBrush { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            double height = (double)value;
            LinearGradientBrush brush = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, height), MappingMode = BrushMappingMode.Absolute, SpreadMethod = GradientSpreadMethod.Repeat };
            brush.GradientStops.Add(new GradientStop() { Color = BackgroundBrush, Offset = (height - 1) / height });
            brush.GradientStops.Add(new GradientStop() { Color = BorderBrush, Offset = (height - 1) / height });
            return brush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}