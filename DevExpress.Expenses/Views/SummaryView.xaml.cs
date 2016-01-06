using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core.Native;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.XtraBars.WinRTLiveTiles;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace Expenses.Wpf
{
    /// <summary>
    /// Interaction logic for SummaryView.xaml
    /// </summary>
    public partial class SummaryView : UserControl
    {
        public static WinRTLiveTileManager LiveTileManager {
            get { return App.LiveTileManager; }
        }
        DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
        public SummaryView()
        {
            InitializeComponent();
            SetTileButtonVisibility();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
            Unloaded += SummaryView_Unloaded;
        }

        void timer_Tick(object sender, EventArgs e) {
            UpdateLiveTile();
        }

        void SummaryView_Unloaded(object sender, RoutedEventArgs e) {
            timer.Stop();
        }

        private void registerTileButton_Click(object sender, RoutedEventArgs e) {
            if(WinRTLiveTileManager.CheckWinRTAppInstalled())
                App.LiveTileManager.ShowLiveTileManager();
            else {
                IDialogService service = RegisterLiveTileDialogService;
                service.ShowDialog(new[] { new UICommand(null, "Close", null, false, true) }, "Warning - Live Tile Manager not found.", null);
            }

        }
        void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Process.Start(@"http://apps.microsoft.com/windows/en-us/app/devexpress-live-tile-manager/acdad900-0021-4cbd-90cd-4ae5a03e91f5");
            Window.GetWindow((DependencyObject)sender).Close();
        }
        void SetTileButtonVisibility() {
            bool isWindows8;
            try {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\kernel32.dll");
                isWindows8 = (info.ProductMajorPart == 6 && (info.ProductMinorPart == 2 || info.ProductMinorPart == 3));
            } catch { isWindows8 = false; }
            registerTileButton.Visibility = isWindows8 ? Visibility.Visible : Visibility.Collapsed;
        }
        private void UpdateLiveTile() {
            if(LiveTileManager == null || !LiveTileManager.HasPinnedTile)
                return;
            LiveTileManager.ClearTile();
            AddTile(largeTileContent1, smallTileContent1);
            AddTile(largeTileContent2, smallTileContent2);
            AddTile(largeTileContent3, smallTileContent3);
            AddTile(largeTileContent4, smallTileContent4);
        }
        void AddTile(UIElement large, UIElement small) {
            WideTile wide = WideTile.CreateTileWideImage(RenderTreeToImage(large));
            SquareTile square = SquareTile.CreateTileSquareImage(RenderTreeToImage(small));
            LiveTileManager.UpdateTile(wide, square, true);
        }
        System.Drawing.Image RenderTreeToImage(UIElement userControl) {
            double height = userControl.RenderSize.Height;
            double width = userControl.RenderSize.Width;
            if(height == 0 || width == 0)
                return new System.Drawing.Bitmap(1, 1);
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Default);
            VisualBrush sourceBrush = new VisualBrush(userControl);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            using(drawingContext) {
                drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(0, 0), new Point(width, height)));
            }
            renderTarget.Render(drawingVisual);
            using(MemoryStream ms = new MemoryStream()) {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                encoder.Save(ms);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                return img;
            }
        }

        private void ChartControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            SeriesPresentation series = LayoutHelper.FindParentObject<SeriesPresentation>(e.OriginalSource as FrameworkElement);
            if(series == null) return;
            Expenses.ViewModel.INavigationService service = ServiceLocator.Current.GetService<Expenses.ViewModel.INavigationService>() as Expenses.ViewModel.INavigationService;
            switch(series.Series.DisplayName) {
                case "Past":                    
                    service.ShowPastExpenseReports();
                    break;
                case "Saved":
                    service.ShowSavedExpenseReports();
                    break;
                case "Pending":
                    service.ShowPendingExpenseReports();
                    break;
            }            
        }

    }
}
