using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expenses.Wpf {
    /// <summary>
    /// Interaction logic for LiveTileSmall.xaml
    /// </summary>
    public partial class LiveTileSmall : LiveTileBase {
        public LiveTileSmall() {
            InitializeComponent();
        }
    }
    public class LiveTileBase : UserControl {

        public string Label1 {
            get { return (string)GetValue(Label1Property); }
            set { SetValue(Label1Property, value); }
        }
        public static readonly DependencyProperty Label1Property =
            DependencyProperty.Register("Label1", typeof(string), typeof(LiveTileBase), new PropertyMetadata("Label1"));




        public string Label2 {
            get { return (string)GetValue(Label2Property); }
            set { SetValue(Label2Property, value); }
        }
        public static readonly DependencyProperty Label2Property =
            DependencyProperty.Register("Label2", typeof(string), typeof(LiveTileBase), new PropertyMetadata("Label2"));

        public double Amount {
            get { return (double)GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }
        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register("Amount", typeof(double), typeof(LiveTileBase), new PropertyMetadata(0d));

        
    }
}
