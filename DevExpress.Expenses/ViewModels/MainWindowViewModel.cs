using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Expenses.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        public readonly IExpenseRepository Repository;

        public ViewModelBase CurrentViewModel
        {
            get { return this._currentViewModel; }
            set
            {
                if (this._currentViewModel == value) { return; }

                this._currentViewModel = value;
                this.NotifyOfPropertyChange(() => this.CurrentViewModel);
            }
        }
        private ViewModelBase _currentViewModel;

        public LogInControlViewModel LogInViewModel
        {
            get { return this._logInViewModel; }
            set
            {
                if (this._logInViewModel == value) { return; }

                this._logInViewModel = value;
                this.NotifyOfPropertyChange(() => this.LogInViewModel);
            }
        }
        private LogInControlViewModel _logInViewModel;

        public EmployeeViewModel EmployeeViewModel { get; set; }

        public bool ShowLogin
        {
            get { return this._showLogin; }
            set
            {
                if (this._showLogin == value) { return; }

                this._showLogin = value;
                this.NotifyOfPropertyChange(() => this.ShowLogin);
            }
        }
        private bool _showLogin;

        public readonly IViewService ViewService;

        public readonly NavigationService NavigationService;

        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void OnRequestChangeUser()
        {
            string alias = (this.EmployeeViewModel.Alias == this.LogInViewModel.Alias) ? this.EmployeeViewModel.Manager : this.LogInViewModel.Alias;            
            this.EmployeeViewModel.Employee = this.Repository.GetEmployee(alias);
            this.ShowSummaryView();    
        }

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                this._isBusy = value;
                this.NotifyOfPropertyChange(() => this.IsBusy);
            }
        }
        private bool _isBusy;

        public ICommand CloseCommand
        {
            get
            {
                if (this._closeCommand == null)
                {
                    this._closeCommand = new RelayCommand(param => this.OnRequestClose());
                }
                return this._closeCommand;
            }
        }
        private ICommand _closeCommand;

        public ICommand ChangeUserCommand
        {
            get
            {
                if (this._changeUserCommand == null)
                {
                    this._changeUserCommand = new RelayCommand((_) => { this.OnRequestChangeUser(); });
                }
                return this._changeUserCommand;
            }
        }
        private ICommand _changeUserCommand;
        
        public ICommand ResetDataCommand { get; private set; }
        public ICommand NewReportCommand { get; private set; }        

        public MainWindowViewModel()
        {
            // TODO: There might be a better way to do this.
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>() as NavigationService;
            this.Repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();
            this.ViewService.BusyChanged += ViewService_BusyChanged;
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();

            this.LogInViewModel = new LogInControlViewModel(this, new RelayCommand((_) => this.LogIn()));
            //NOTE: Automatically logging in hardcoded alias for now
            this.LogInViewModel.LogInCommand.Execute(this);

            this.ResetDataCommand = new RelayCommand((_) => this.ResetData());
            this.NewReportCommand = new RelayCommand((_) => this.NewReport());            

            this.NavigationService.ShowChargeRequested += (_, ea) => { this.ShowCharge(ea.Data); };
            this.NavigationService.ShowChargesRequested += (_, __) => { this.ShowCharges(); };
            this.NavigationService.ShowExpenseReportRequested += (_, ea) => { this.ShowExpenseReport(ea.Data); };
            this.NavigationService.ShowPastExpenseReportsRequested += (_, __) => { this.ShowPastReports(); };
            this.NavigationService.ShowPendingExpenseReportsRequested += (_, __) => { this.ShowPendingReports(); };
            this.NavigationService.ShowSavedExpenseReportsRequested +=  (_, __) => { this.ShowSavedReports(); };
            this.NavigationService.ShowSummaryRequested += (_, __) => { this.ShowSummaryView(); };
            this.NavigationService.ShowReportsForApprovalRequested += (_, __) => { this.ShowReportsForApproval(); };
            this.NavigationService.ChangeUserRequested += (_, __) => { this.OnRequestChangeUser(); };
            this.NavigationService.CreateNewReportRequested += (_, __) => { this.NewReport(); };
            //this.ShowLogin = true;
        }

        void ViewService_BusyChanged(object sender, EventArgs<bool> e)
        {
            this.IsBusy = e.Data;
        }

        public void ShowCharge(ChargeViewModel charge)
        {
            this.CurrentViewModel = charge;
        }

        public void NewReport()
        {
            ExpenseReportViewModel reportVM = new ExpenseReportViewModel()
            {
                Approver = this.EmployeeViewModel.Manager,
                EmployeeId = this.EmployeeViewModel.EmployeeId,
                ExpenseReportId = 0,
            };

            this.ShowExpenseReport(reportVM);
        }

        public void LogIn()
        {            
            this.EmployeeViewModel.Employee = this.Repository.GetEmployee(this.LogInViewModel.Alias);
            this.ShowSummaryView();
            this.ShowLogin = string.IsNullOrWhiteSpace(this.EmployeeViewModel.Alias);            
        }

        public void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel) {
            
            var editReportVM = new EditExpenseReportViewModel();
            editReportVM.ExpenseReport = expenseReportViewModel;

            AddChargesViewModel addChargesVM = new AddChargesViewModel();
            addChargesVM.LoadCharges();
            editReportVM.AddCharges = addChargesVM;

            ExpenseReportChargesViewModel associatedChargesVM = new ExpenseReportChargesViewModel();
            associatedChargesVM.LoadCharges(expenseReportViewModel.ExpenseReportId);
            editReportVM.AssociatedCharges = associatedChargesVM;

            this.CurrentViewModel = editReportVM;
            
        }

        public void ShowReportsForApproval()
        {            
            ReportsViewModel reportsViewModel = GetReportsViewModel();
            reportsViewModel.CurrentViewType = ReportsSubviewType.ApprovalsReports;                    
            (reportsViewModel.CurrentViewModel as ApproveExpenseReportsViewModel).LoadReportsForApproval();
            this.CurrentViewModel = reportsViewModel;            
        }
        ReportsViewModel GetReportsViewModel() {
            return this.CurrentViewModel is ReportsViewModel ? this.CurrentViewModel as ReportsViewModel : new ReportsViewModel();
        }
        public void ShowCharges() {            
            ReportsViewModel reportsViewModel = GetReportsViewModel();
            reportsViewModel.CurrentViewType = ReportsSubviewType.OutgoingCharges;                    
            (reportsViewModel.CurrentViewModel as ChargesViewModel).LoadCharges();
            this.CurrentViewModel = reportsViewModel;
        }        
        public void ShowSummaryView() {            
            SummaryItemsViewModel summaryViewModel = new SummaryItemsViewModel(this.EmployeeViewModel);
            summaryViewModel.GetSummaryItems();
            this.CurrentViewModel = summaryViewModel;            
        }

        public void ShowSavedReports() {
            ReportsViewModel reportsViewModel = GetReportsViewModel();
            reportsViewModel.CurrentViewType = ReportsSubviewType.SavedReports;
            (reportsViewModel.CurrentViewModel as ExpenseReportsViewModel).LoadSavedExpenseReports();
            this.CurrentViewModel = reportsViewModel;
        }

        public void ShowPendingReports() {
            ReportsViewModel reportsViewModel = GetReportsViewModel();
            reportsViewModel.CurrentViewType = ReportsSubviewType.PendingReports;
            (reportsViewModel.CurrentViewModel as ExpenseReportsViewModel).LoadPendingExpenseReports();
            this.CurrentViewModel = reportsViewModel;
        }

        public void ShowPastReports() {
            ReportsViewModel reportsViewModel = GetReportsViewModel();
            reportsViewModel.CurrentViewType = ReportsSubviewType.PastReports;                    
            (reportsViewModel.CurrentViewModel as  ExpenseReportsViewModel).LoadApprovedExpenseReports();
            this.CurrentViewModel = reportsViewModel;
        }

        public void ResetData()
        {
            this.Repository.ResetData();
            this.LogIn();
            this.ShowCharges();   
        }
    }
}
