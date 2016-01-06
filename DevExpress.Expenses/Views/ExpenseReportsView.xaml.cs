using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Grid;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using Expenses.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Expenses.Wpf {
    /// <summary>
    /// Interaction logic for ExpenseReportsView.xaml
    /// </summary>
    public partial class ExpenseReportsView : UserControl {
        const double explodeDistance = 0.1;

        DispatcherTimer timer = new DispatcherTimer();

        public ExpenseReportsView() {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += timer_Tick;
            DataContextChanged += ExpenseReportsView_DataContextChanged;
        }

        void timer_Tick(object sender, EventArgs e) {
            if(chart.Diagram.Series[0].Points.Count > 0) {
                PieSeries2D.SetExplodedDistance(chart.Diagram.Series[0].Points[0], explodeDistance);
                UpdateChargeInfo(chart.Diagram.Series[0].Points[0]);
            }
            timer.Stop();
        }

        void ExpenseReportsView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (e.NewValue is ExpenseReportsViewModel)
                ((ExpenseReportsViewModel)e.NewValue).ShowExpenseReport += ExpenseReportsView_ShowExpenseReport;
            if (e.OldValue is ExpenseReportsViewModel)
                ((ExpenseReportsViewModel)e.OldValue).ShowExpenseReport -= ExpenseReportsView_ShowExpenseReport;
        }

        void ExpenseReportsView_ShowExpenseReport(object sender, EventArgs e) {
            if(sender == null)
                return;
            DevExpress.Mvvm.IDialogService service = ViewReportDialogService;
            ExpenseReportViewModel expenseReportViewModel = (ExpenseReportViewModel)sender;

            var editReportVM = new EditExpenseReportViewModel();
            editReportVM.ExpenseReport = expenseReportViewModel;

            AddChargesViewModel addChargesVM = new AddChargesViewModel();
            addChargesVM.LoadCharges();
            editReportVM.AddCharges = addChargesVM;

            ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel();
            associatedChargesVM.LoadCharges(expenseReportViewModel.ExpenseReportId);
            editReportVM.AssociatedCharges = associatedChargesVM;

            List<UICommand> commands = new List<UICommand>();
            if(DataContext is ApproveExpenseReportsViewModel) {
                ICommand approveCommand = new DelegateCommand<CancelEventArgs>(x => {
                    //if(ApproveDialogService.ShowDialog(MessageBoxButton.OKCancel, "Confirm expense report", null) == MessageBoxResult.OK) {
                        expenseReportViewModel.ApproveReportCommand.Execute(null);
                    //} else {
                    //    x.Cancel = true;
                    //}
                });
                commands.Add(new UICommand(null, "Approve", approveCommand, true, false));
            }
            if(editReportVM.CanSubmit)
                commands.Add(new UICommand(null, "Submit", expenseReportViewModel.SubmitReportCommand, true, false));

            UICommand cancelCommand = new UICommand(null, "Close", null, false, true);
            commands.Add(cancelCommand);


            if(service.ShowDialog(commands, expenseReportViewModel.Purpose + " report", editReportVM) != cancelCommand) {
                expenseReportViewModel.ExpenseReportsViewModel.OnExpenseReportApproved(editReportVM);
            }

        }
        void UpdateChargeInfo(SeriesPoint point) {
            ((ExpenseReportsViewModel)DataContext).SelectedChargeCategory = point.Argument;
            ((ExpenseReportsViewModel)DataContext).SelectedChargeValue = point.Value;
        }
        void ChartControl_BoundDataChanged(object sender, RoutedEventArgs e) {
            timer.Start();
        }
        void chart_MouseUp(object sender, MouseButtonEventArgs e) {
            ChartHitInfo hitInfo = chart.CalcHitInfo(e.GetPosition(chart));
            if(hitInfo == null || hitInfo.SeriesPoint == null || PieSeries.GetExplodedDistance(hitInfo.SeriesPoint) > 0)
                return;
            foreach(SeriesPoint point in chart.Diagram.Series[0].Points) {
                if(PieSeries.GetExplodedDistance(point) > 0 && point != hitInfo.SeriesPoint)
                    AnimateExploding(point, true);
            }
            AnimateExploding(hitInfo.SeriesPoint, false);
            UpdateChargeInfo(hitInfo.SeriesPoint);
        }
        void AnimateExploding(SeriesPoint seriesPoint, bool isDealayedStart) {
            Storyboard storyBoard = new Storyboard();
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            if(isDealayedStart) {
                animation.KeyFrames.Add(new LinearDoubleKeyFrame(explodeDistance, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
                animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 600))));
            } else {
                double distanceTo = PieSeries.GetExplodedDistance(seriesPoint) > 0 ? 0 : explodeDistance;
                animation.KeyFrames.Add(new LinearDoubleKeyFrame(distanceTo, KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0, 0, 300))));
            }
            storyBoard.Children.Add(animation);
            Storyboard.SetTarget(animation, seriesPoint);
            Storyboard.SetTargetProperty(animation, new PropertyPath(PieSeries.ExplodedDistanceProperty));
            storyBoard.Begin();
        }

        private void exportButton_Click(object sender, RoutedEventArgs e) {
            DevExpress.Mvvm.IDialogService service = ExportDialogService;

            UICommand[] commands = new UICommand[] { new UICommand(null, "Cancel", null, false, true) };
            service.ShowDialog(commands, "Choose the format you want to export to", ViewModelSource.Create(() => new ExportViewModel(ReportsGrid)));
        }
    }

    public class ExportViewModel {
        protected virtual ICurrentWindowService WindowService { get { return null; } }

        readonly GridControl grid;
        public ExportViewModel(GridControl grid) {
            this.grid = grid;
        }
        public void Export(string format) {
            Window wnd = Window.GetWindow(((CurrentWindowService)WindowService).AssociatedObject);
            wnd.Tag = format;
            wnd.Closed += wnd_Closed;
            wnd.Close();                                    
        }

        void wnd_Closed(object sender, EventArgs e) {
            ((Window)sender).Closed -= wnd_Closed;
            LinkPreviewModel model = new LinkPreviewModel(new PrintableControlLink(grid.View as IPrintableControl));
            model.Link.PrintingSystem.ExportOptions.PrintPreview.ShowOptionsBeforeExport = false;
            model.Link.PrintingSystem.ExportOptions.PrintPreview.ActionAfterExport = DevExpress.XtraPrinting.ActionAfterExport.None;
            model.Link.CreateDocument(false);
            model.ExportCommand.Execute(((Window)sender).Tag);
        }
    }
    public class LegendPanel : Panel {
        double itemWidth = 0.0;
        double itemHeight = 0.0;

        protected override Size MeasureOverride(Size availableSize) {
            if(Children.Count == 0) return new Size();
            Size infinity = new Size(double.PositiveInfinity, double.PositiveInfinity);
            double itemWidth = 0.0;
            double itemHeight = 0.0;
            foreach(UIElement child in Children) {
                child.Measure(infinity);
                itemWidth = Math.Max(itemWidth, child.DesiredSize.Width);
                itemHeight = Math.Max(itemHeight, child.DesiredSize.Height);
            }
            int columns = (int)Math.Floor(availableSize.Width / itemWidth);
            int rows = (Children.Count + columns - 1) / columns;
            this.itemWidth = itemWidth;
            this.itemHeight = itemHeight;
            return new Size(columns * itemWidth, rows * itemHeight);
        }
        protected override Size ArrangeOverride(Size finalSize) {
            if(Children.Count == 0) return finalSize;
            int columns = (int)Math.Floor(finalSize.Width / itemWidth);
            double x = 0.0;
            double y = 0.0;
            foreach(UIElement child in Children) {
                child.Arrange(new Rect(new Point(x, y), new Size(itemWidth, itemHeight)));
                x += itemWidth;
                if(x >= finalSize.Width) {
                    x = 0.0;
                    y += itemHeight;
                }
            }
            return finalSize;
        }
    }

    public class ViewTypeToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if(parameter != null) {
                EnumConverter conv = new EnumConverter(typeof(ReportsSubviewType));
                object param = conv.ConvertFrom(parameter);
                return value.Equals(param) ? Visibility.Visible : Visibility.Collapsed;
            }
            return value.Equals(ReportsSubviewType.SavedReports) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
