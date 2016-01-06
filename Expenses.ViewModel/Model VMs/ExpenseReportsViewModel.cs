//*********************************************************
// Copyright (c) Microsoft. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

//*********************************************************
// Copyright (c) DevExpress, Inc. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class ExpenseReportsViewModel : ViewModelBase
    {
        public ObservableCollection<SortType> SortTypes { get; set; }

        public ObservableCollection<GroupInfoList<object>> GroupedExpenseReports
        {
            get { return this._groupedExpenseReports; }
            set
            {
                this._groupedExpenseReports = value;
                this.NotifyOfPropertyChange(() => this.GroupedExpenseReports);
            }
        }
        private ObservableCollection<GroupInfoList<object>> _groupedExpenseReports;

        public ObservableCollection<ExpenseReportViewModel> ExpenseReports
        {
            get { return this._expenseReports; }
            set
            {
                this._expenseReports = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReports);
            }
        }
        private ObservableCollection<ExpenseReportViewModel> _expenseReports;

        public IEnumerable<ExpenseReportViewModel> FilteredExpenseReports {
            get { return this._FilteredExpenseReports; }
            set {
                this._FilteredExpenseReports = value;
                this.NotifyOfPropertyChange(() => this.FilteredExpenseReports);
            }
        }
        private IEnumerable<ExpenseReportViewModel> _FilteredExpenseReports;

        //public IEnumerable<ExpenseCategory> FilteredExpenseCategories {
        //    get { return this._FilteredExpenseCategories; }
        //    set {
        //        this._FilteredExpenseCategories = value;
        //        this.NotifyOfPropertyChange(() => this.FilteredExpenseCategories);
        //    }
        //}
        //private IEnumerable<ExpenseCategory> _FilteredExpenseCategories;

        public IEnumerable<Tuple<string, decimal>> FilteredExpenseCharges {
            get { return this._FilteredExpenseCharges; }
            set {
                this._FilteredExpenseCharges = value;
                this.NotifyOfPropertyChange(() => this.FilteredExpenseCharges);
            }
        }
        private IEnumerable<Tuple<string, decimal>> _FilteredExpenseCharges;

        public DateTime? StartDate {
            get { return this._StartDate; }
            set {
                this._StartDate = value;
                this.NotifyOfPropertyChange(() => this.StartDate);
            }
        }
        DateTime? _StartDate;

        public DateTime? EndDate {
            get { return this._EndDate; }
            set {
                this._EndDate = value;
                this.NotifyOfPropertyChange(() => this.EndDate);
            }
        }
        DateTime? _EndDate;

        public DateTime? SelectionStartDate {
            get { return this._SelectionStartDate; }
            set {
                if(this._SelectionStartDate == value) return;
                this._SelectionStartDate = value;
                this.NotifyOfPropertyChange(() => this.SelectionStartDate);
                UpdateFilteredExpenseReports();
            }
        }
        DateTime? _SelectionStartDate;

        public DateTime? SelectionEndDate {
            get { return this._SelectionEndDate; }
            set {
                if(this._SelectionEndDate == value) return;
                this._SelectionEndDate = value;
                this.NotifyOfPropertyChange(() => this.SelectionEndDate);
                UpdateFilteredExpenseReports();
            }
        }
        DateTime? _SelectionEndDate;

        public string SelectedChargeCategory {
            get { return this._SelectedChargeCategory; }
            set {
                this._SelectedChargeCategory = value;
                this.NotifyOfPropertyChange(() => this.SelectedChargeCategory);
          }
        }
        string _SelectedChargeCategory;

        public double SelectedChargeValue {
            get { return this._SelectedChargeValue; }
            set {
                this._SelectedChargeValue = value;
                this.NotifyOfPropertyChange(() => this.SelectedChargeValue);
            }
        }
        double _SelectedChargeValue;

        public SortType SortType
        {
            get { return this._sortType; }
            set
            {
                if (this._sortType == value) { return; }

                _sortType = value;
                this.NotifyOfPropertyChange(() => this.SortType);

                //update the grouping because the sort/filter type changed
                this.GroupExpenseReports();
            }
        }
        private SortType _sortType = SortType.ItemType;

        public readonly IExpenseRepository _repository;
        public readonly INavigationService NavigationService;
        public readonly EmployeeViewModel EmployeeViewModel;
        private readonly IViewService ViewService;

        public ICommand ViewReportCommand { get; private set; }
        public ICommand NewReportCommand { get; private set; }
        public ICommand ApproveReportCommand { get; private set; }
        

        public ExpenseReportsViewModel()
        {
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            this._expenseReports = new ObservableCollection<ExpenseReportViewModel>();
            this._groupedExpenseReports = new ObservableCollection<GroupInfoList<object>>();
            this.SortTypes = new ObservableCollection<SortType>();
            this.SortTypes.Add(SortType.Age);
            this.SortTypes.Add(SortType.Amount);

            this.ViewReportCommand = new RelayCommand(
                (report) =>
                {
                    this.ViewReport(report as ExpenseReportViewModel);
                });
            this.NewReportCommand = new RelayCommand(
                o => {
                    NavigationService.CreateNewReport();
                });

            this.ApproveReportCommand =
                new RelayCommand(
                    async (report) => {
                        await this.ApproveExpenseReportAsync(report as ExpenseReportViewModel);
                        this.LoadReportsForApproval();
                    });

            this._viewService = ServiceLocator.Current.GetService<IViewService>();

        }

        public event EventHandler ShowExpenseReport;

        private void ViewReport(ExpenseReportViewModel reportViewModel)
        {
            if(ShowExpenseReport != null)
                ShowExpenseReport(reportViewModel, EventArgs.Empty);
            //this.NavigationService.ShowExpenseReport(reportViewModel);   
        }

        protected void Load(IEnumerable<ExpenseReport> expenseReports)
        {
            this.ExpenseReports.Clear();
            foreach(ExpenseReport expenseReport in expenseReports)
            {
                this.ExpenseReports.Add(new ExpenseReportViewModel(this, expenseReport));
            }
            if(ExpenseReports.Any()) {
                DateTime minDate = ExpenseReports.Min(x => x.DisplayDate);
                StartDate = new DateTime(minDate.Year, minDate.Month, 1);
                DateTime maxDate = ExpenseReports.Max(x => x.DisplayDate);
                EndDate = new DateTime(maxDate.Year, maxDate.Month, 1).AddMonths(1);
            } else {
                StartDate = DateTime.Now.AddYears(-1);
                EndDate = DateTime.Now.AddMonths(1);
            }
            SelectionStartDate = StartDate;
            SelectionEndDate = EndDate;

        }
        static bool once = false;
        void UpdateFilteredExpenseReports() {
            FilteredExpenseReports = ExpenseReports.Where(x => x.DisplayDate >= SelectionStartDate && x.DisplayDate <= SelectionEndDate).ToArray();
            IEnumerable<ChargeViewModel> charges = GetAllCharges(FilteredExpenseReports);
            FilteredExpenseCharges = charges.GroupBy(c => c.CategoryName).Select(g => new Tuple<string, decimal>(g.Key, g.Select(k => k.BilledAmount).Sum())).ToArray();
        }
        IEnumerable<ChargeViewModel> GetAllCharges(IEnumerable<ExpenseReportViewModel> reports) {
            // TODO A Add method to the service
            List<ChargeViewModel> c = new List<ChargeViewModel>();
            foreach(ExpenseReportViewModel v in reports) {
                foreach(Charge m in _repository.GetCharges(v.ExpenseReportId)) {
                    c.Add(new ChargeViewModel() { Charge = m });
                }
            }
            return c;
        }

        enum ReportsType { Saved, Pending, Approved, ForApproval }
        ReportsType reportsType;

        //public async Task LoadAllExpenseReportsAsync()
        //{
        //    await this.ViewService.ExecuteBusyActionAsync(
        //        async () =>
        //        {
        //            this.Load(await this._repository.GetExpenseReportsAsync(this.EmployeeViewModel.EmployeeId));
        //        });
        //}

        public void LoadSavedExpenseReports() {
            reportsType = ReportsType.Saved;
            this.Load(this._repository.GetExpenseReportsByStatus(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Saved));
        }

        public void LoadPendingExpenseReports() {            
            this.Load(this._repository.GetExpenseReportsByStatus(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Pending));
        }

        public void LoadApprovedExpenseReports() {
            this.Load(this._repository.GetExpenseReportsByStatus(this.EmployeeViewModel.EmployeeId, ExpenseReportStatus.Approved));
            GroupApprovedExpenseReportsByAge();
        }

        public void LoadReportsForApproval() {
            this.Load(this._repository.GetExpenseReportsForApproval(this.EmployeeViewModel.Alias));
        }

        public void GroupExpenseReports()
        {
            switch (this.SortType)
            {
                case SortType.Category:
                    GroupExpenseReportsByCategory();
                    break;
                case SortType.Age:
                    GroupExpenseReportsByAge();
                    break;
                case SortType.Amount:
                    GroupExpenseReportsByAmount();
                    break;
                case SortType.Submitter:
                    GroupExpenseReportsBySubmitter();
                    break;
                case SortType.ItemType:
                    GroupExpenseReportsByAge();
                    break;
                default:
                    break;
            }
        }

        internal void GroupApprovedExpenseReportsByAge()
        {
            var query = from expenseReport in this.ExpenseReports
                        orderby expenseReport.DateResolved descending
                        group expenseReport by ((DateTime)expenseReport.DateResolved).Year into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedExpenseReports.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                items.Key = group.GroupName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.Summary = string.Format("{0} for {1:C}",
                    group.Count, items.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedExpenseReports.Add(items);
            }
        }

        internal GroupInfoList<object> GroupExpenseReportsByCategory()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.Status
                        group expenseReport by expenseReport.Status into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var groupedExpenses = new GroupInfoList<object>();
                string statusTypeName = null;
                ExpenseReportStatus statusType = (ExpenseReportStatus)group.GroupName;
                switch (statusType)
                {
                    case ExpenseReportStatus.Approved:
                        statusTypeName = "Approved";
                        break;
                    case ExpenseReportStatus.Pending:
                        statusTypeName = "Pending";
                        break;
                    case ExpenseReportStatus.Saved:
                        statusTypeName = "Saved";
                        break;
                    default:
                        break;
                }
                groupedExpenses.Key = string.Format(
                    "{0} ({1} items for {2:C})", statusTypeName, group.Count, group.Amount);
                groupedExpenses.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (var item in groupedExpenses)
                {
                    groupedExpenses.Add(item);
                }
                groups.Add(groupedExpenses);
            }

            return groups;
        }

        internal void GroupExpenseReportsByAge()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.DateResolved descending
                        group expenseReport by ((DateTime)expenseReport.DateResolved).Year into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedExpenseReports.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                items.Key = group.GroupName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.Summary = string.Format("{0} for {1:C}",
                    group.Count, items.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedExpenseReports.Add(items);
            }
        }

        internal void GroupExpenseReportsByAmount()
        {
        }

        internal GroupInfoList<object> GroupExpenseReportsBySubmitter()
        {
            var query = from expenseReport in ExpenseReports
                        orderby expenseReport.Status
                        group expenseReport by expenseReport.EmployeeId into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var groupedExpenses = new GroupInfoList<object>();
                groupedExpenses.Key = string.Format(
                    "{0} ({1} items for {2:C})", group.GroupName, group.Count, group.Amount);
                groupedExpenses.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (var item in groupedExpenses)
                {
                    groupedExpenses.Add(item);
                }
                groups.Add(groupedExpenses);
            }

            return groups;
        }



        private IViewService _viewService;


        private async Task ApproveExpenseReportAsync(ExpenseReportViewModel reportViewModel) {
            if(await this._viewService.ConfirmAsync(
                "Confirm expense report",
                "Are you sure you want to approve this expense report?")) {
                reportViewModel.Approve();
            }
        }

        public void Reload() {
            switch(reportsType) {
                case ReportsType.Saved:
                    LoadSavedExpenseReports();
                    break;
                case ReportsType.Pending:
                    LoadPendingExpenseReports();
                    break;
                case ReportsType.Approved:
                    LoadApprovedExpenseReports();
                    break;
                case ReportsType.ForApproval:
                    LoadReportsForApproval();
                    break;
                default:
                    break;
            }
        }

        public void OnExpenseReportApproved(EditExpenseReportViewModel editReportVM) {
            LoadReportsForApproval();
            UpdateFilteredExpenseReports();
        }
    }

}
