using DevExpress.Xpf.Grid;
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

namespace Expenses.Wpf
{
    /// <summary>
    /// Interaction logic for ExpenseReportChargesView.xaml
    /// </summary>
    public partial class ExpenseReportChargesView : UserControl
    {

        public GridViewNavigationStyle NavigationStyle {
            get { return (GridViewNavigationStyle)GetValue(GridSelectionModeProperty); }
            set { SetValue(GridSelectionModeProperty, value); }
        }

        public static readonly DependencyProperty GridSelectionModeProperty =
            DependencyProperty.Register("NavigationStyle", typeof(GridViewNavigationStyle), typeof(ExpenseReportChargesView), new PropertyMetadata(GridViewNavigationStyle.Row));
        

        public ExpenseReportChargesView() {
            
            InitializeComponent();
        }
    }
}
