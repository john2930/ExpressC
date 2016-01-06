using DevExpress.Xpf.Grid;
using Expenses.ViewModel;
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
    /// Interaction logic for EditExpenseReportView.xaml
    /// </summary>
    public partial class EditExpenseReportView : UserControl
    {
        public EditExpenseReportView()
        {
            InitializeComponent();
        }


        void MoveItems(GridControl grid, ICommand command) {
            foreach(var item in grid.SelectedItems.Cast<object>().ToArray()) {
                command.Execute(item);
            }
        }
        private void addChargesButton_Click(object sender, RoutedEventArgs e) {
            MoveItems(outstandingChargesGrid, ((EditExpenseReportViewModel)DataContext).AddChargeToReportCommand);
        }

        private void removeChargesButton_Click(object sender, RoutedEventArgs e) {
            MoveItems(reportChargesGrid, ((EditExpenseReportViewModel)DataContext).RemoveChargeFromReportCommand);
        }


        Point startPoint;
        int rowHandle = DataControlBase.InvalidRowHandle;
        void GridControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            GridControl grid = sender as GridControl;
            if(grid.SelectionMode != MultiSelectMode.Row) return;
            rowHandle = grid.View.GetRowHandleByMouseEventArgs(e);
            startPoint = e.GetPosition(this);
        }
        const double maxOffset = 10;
        void GridControl_MouseMove(object sender, MouseEventArgs e) {
            GridControl grid = sender as GridControl;
            if(grid.SelectionMode != MultiSelectMode.Row) return;
            Point currentPoint = e.GetPosition(this);
            if(currentPoint.X <= startPoint.X - maxOffset || currentPoint.X >= startPoint.X + maxOffset ||
               currentPoint.Y <= startPoint.Y - maxOffset || currentPoint.Y >= startPoint.Y + maxOffset)
                rowHandle = DataControlBase.InvalidRowHandle;
        }
        void GridControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            GridControl grid = sender as GridControl;
            if(grid.SelectionMode != MultiSelectMode.Row) return;
            if(rowHandle != grid.View.GetRowHandleByMouseEventArgs(e) || rowHandle == DataControlBase.InvalidRowHandle) return;
            if(!grid.View.IsRowSelected(rowHandle))
                grid.SelectItem(rowHandle);
            else
                grid.UnselectItem(rowHandle);
            rowHandle = DataControlBase.InvalidRowHandle;
        }
    }
}
