using DevExpress.Xpf.Core;
using DevExpress.XtraBars.WinRTLiveTiles;
using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Expenses.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ThemeManager.ApplicationThemeName = Theme.TouchlineDarkName;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitLiveTileManager();

            MainWindow mainWindow = new MainWindow();

            // TODO: Should probably be initialized somewhere else.
            // string url = ConfigurationManager.AppSettings["expenseServiceUrl"];
            LocalExpenseRepository localRepository = new LocalExpenseRepository();
            FileInfo localDataFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data2.bin"));
            if(localDataFile.Exists) {
                using(FileStream stream = localDataFile.OpenRead()) {
                    localRepository.Load(stream);
                }
            }
            else localRepository.Create();
            // localRepository.ResetData();

            ServiceLocator.Current.SetService<IExpenseRepository>(localRepository);
            ServiceLocator.Current.SetService<IViewService>(new ViewService());
            ServiceLocator.Current.SetService<INavigationService>(new NavigationService());
            ServiceLocator.Current.SetService<EmployeeViewModel>(new EmployeeViewModel());

            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainViewModel;

            EventHandler handler = null;
            handler = delegate
            {
                mainViewModel.RequestClose -= handler;
                mainWindow.Close();
            };
            mainViewModel.RequestClose += handler;

            mainViewModel.ViewService.BusyChanged +=
                (_, ea) =>
                {
                    mainWindow.Cursor = (ea.Data) ? Cursors.Wait : Cursors.Arrow;
                };

            mainWindow.Show();
        }

        void InitLiveTileManager() { liveTileManager = CreateLiveTileManager(); }
        static WinRTLiveTileManager liveTileManager;
        public static WinRTLiveTileManager LiveTileManager {
            get {
                if(liveTileManager == null)
                    liveTileManager = CreateLiveTileManager();
                return liveTileManager;
            }
        }
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            FileInfo localDataFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data2.bin"));
            using(FileStream stream = localDataFile.Create()) {
                ((LocalExpenseRepository)ServiceLocator.Current.GetService<IExpenseRepository>()).Save(stream);
            }
        }
        static WinRTLiveTileManager CreateLiveTileManager() {
            IContainer components = new System.ComponentModel.Container();
            WinRTLiveTileManager res = new WinRTLiveTileManager(components);
            ((ISupportInitialize)res).BeginInit();
            res.DefaultTileImage = System.Drawing.Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("DevExpress.Expenses.Views.Images.DefaultTileLarge.png"));
            res.ApplicationName = "Expense Manager";
            res.Id = "2F1C8FFB-F055-410E-A94A-43E7202125F5";
            ((ISupportInitialize)res).EndInit();
            return res;
        }
    }
}
