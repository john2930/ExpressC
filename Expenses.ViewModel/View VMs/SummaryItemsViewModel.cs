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
using Expenses;
using Expenses.Model;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class SummaryItemsViewModel : ViewModelBase
    {
        #region Properties

        public ObservableCollection<SortType> SortTypes { get; set; }

        public ObservableCollection<GroupInfoList<object>> GroupedSummaryItems
        {
            get { return this.groupedSummaryItems; }
            set
            {
                this.groupedSummaryItems = value;
                this.NotifyOfPropertyChange(() => this.GroupedSummaryItems);
            }
        }

        public ObservableCollection<SummaryItemViewModel> ListOfAmountOfCharges
        {
            get { return this.listOfAmountOfCharges; }
            set
            {
                this.listOfAmountOfCharges = value;
                this.NotifyOfPropertyChange(() => this.ListOfAmountOfCharges);
            }
        }
        public ObservableCollection<SummaryItemViewModel> AmountMonthlyHistory {
            get { return this.amountMonthlyHistory; }
            set {
                this.amountMonthlyHistory = value;
                this.NotifyOfPropertyChange(() => this.amountMonthlyHistory);
            }
        }
        public ObservableCollection<SummaryItemViewModel> SavedReportsMonthlyHistory {
            get { return this.savedReportsMonthlyHistory; }
            set {
                this.savedReportsMonthlyHistory = value;
                this.NotifyOfPropertyChange(() => this.savedReportsMonthlyHistory);
            }
        }
        public ObservableCollection<SummaryItemViewModel> PastReportsMonthlyHistory {
            get { return this.pastReportsMonthlyHistory; }
            set {
                this.pastReportsMonthlyHistory = value;
                this.NotifyOfPropertyChange(() => this.pastReportsMonthlyHistory);
            }
        }
        public ObservableCollection<SummaryItemViewModel> PendingReportsMonthlyHistory {
            get { return this.pendingReportsMonthlyHistory; }
            set {
                this.pendingReportsMonthlyHistory = value;
                this.NotifyOfPropertyChange(() => this.pendingReportsMonthlyHistory);
            }
        }
        public ObservableCollection<SummaryItemViewModel> SummaryItems
        {
            get { return this.summaryItems; }
            set
            {
                this.summaryItems = value;
                this.NotifyOfPropertyChange(() => this.SummaryItems);
            }
        }

        public SortType SortType
        {
            get { return sortType; }
            set
            {
                if (this.sortType == value) { return; }

                this.sortType = value;
                this.NotifyOfPropertyChange(() => this.SortType);

                //update the grouping because the sort/filter type changed
                this.GroupItems();
            }
        }
                
        public int NumberOfCharges
        {
            get { return this._numberOfCharges; }

            set
            {
                if (this._numberOfCharges == value) { return; }

                this._numberOfCharges = value;
                this.NotifyOfPropertyChange(() => this.NumberOfCharges);
            }
        }
        private int _numberOfCharges = 0;
        
                
        public decimal AmountOfCharges
        {
            get
            { return this._amountOfCharges; }

            set
            {
                if (this._amountOfCharges == value) { return; }

                this._amountOfCharges = value;
                this.NotifyOfPropertyChange(() => this.AmountOfCharges);
            }
        }
        private decimal _amountOfCharges = 0M;

        public int NumberOfChargesLT30Days
        {
            get { return this._numberOfChargesLT30Days; }
            set
            {
                if (this._numberOfChargesLT30Days == value) { return; }

                this._numberOfChargesLT30Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfChargesLT30Days);
            }
        }
        private int _numberOfChargesLT30Days = 0;
                
        public decimal AmountOfChargesLT30Days
        {
            get { return this._amountOfChargesLT30Days; }
            set
            {
                if (this._amountOfChargesLT30Days == value) { return; }

                this._amountOfChargesLT30Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfChargesLT30Days);
            }
        }
        private decimal _amountOfChargesLT30Days = 0M;

        public int NumberOfCharges30To44Days
        {
            get { return this._numberOfCharges30To44Days; }
            set
            {
                if (this._numberOfCharges30To44Days == value) { return; }

                this._numberOfCharges30To44Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfCharges30To44Days);
            }
        }
        private int _numberOfCharges30To44Days = 0;

        public decimal AmountOfCharges30To44Days
        {
            get { return this._amountOfCharges30To44Days; }
            set
            {
                if (this._amountOfCharges30To44Days == value) { return; }

                this._amountOfCharges30To44Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfCharges30To44Days);
            }
        }
        private decimal _amountOfCharges30To44Days = 0M;

        public int NumberOfChargesGT45Days
        {
            get { return this._numberOfChargesGT45Days; }

            set
            {
                if (this._numberOfChargesGT45Days == value) { return; }

                this._numberOfChargesGT45Days = value;
                this.NotifyOfPropertyChange(() => this.NumberOfChargesGT45Days);
            }
        }
        private int _numberOfChargesGT45Days = 0;

        public decimal AmountOfChargesGT45Days
        {
            get { return this._amountOfChargesGT45Days; }

            set
            {
                if (this._amountOfChargesGT45Days == value) { return; }

                this._amountOfChargesGT45Days = value;
                this.NotifyOfPropertyChange(() => this.AmountOfChargesGT45Days);
            }
        }
        private decimal _amountOfChargesGT45Days = 0M;

        private DateTime _beginDateOfHistory;
        private DateTime _endDateOfHistory;
        private DateTime _historySelectionStartDate;
        private DateTime _historySelectionEndDate;
        public DateTime BeginDateOfHistory {
            get { return this._beginDateOfHistory; }
            set {
                if(this._beginDateOfHistory == value) { return; }

                this._beginDateOfHistory = value;
                this.NotifyOfPropertyChange(() => this._beginDateOfHistory);
            }
        }
        public DateTime EndDateOfHistory {
            get { return this._endDateOfHistory; }
            set {
                if(this._endDateOfHistory == value) { return; }

                this._endDateOfHistory = value;
                this.NotifyOfPropertyChange(() => this._endDateOfHistory);
            }
        }
        public DateTime HistorySelectionStartDate {
            get { return this._historySelectionStartDate; }
            set {
                if(this._historySelectionStartDate == value) { return; }

                this._historySelectionStartDate = value;
                this.NotifyOfPropertyChange(() => this.HistorySelectionStartDate);
            }
        }
        public DateTime HistorySelectionEndDate {
            get { return this._historySelectionEndDate; }
            set {
                if(this._historySelectionEndDate == value) { return; }

                this._historySelectionEndDate = value;
                this.NotifyOfPropertyChange(() => this.HistorySelectionEndDate);
            }
        }
        public DateTime BeginDateOfCharges
        {
            get { return this._beginDateOfCharges; }
            set
            {
                if (this._beginDateOfCharges == value) { return; }

                this._beginDateOfCharges = value;
                this.NotifyOfPropertyChange(() => this.BeginDateOfCharges);
            }
        }
        private DateTime _beginDateOfCharges;

        public DateTime EndDateOfCharges
        {
            get { return this._endDateOfCharges; }
            set
            {
                if (this._endDateOfCharges == value) { return; }

                this._endDateOfCharges = value;
                this.NotifyOfPropertyChange(() => this.EndDateOfCharges);
            }
        }
        private DateTime _endDateOfCharges;

        private int numberOfSavedReports = 0;
        public int NumberOfSavedReports
        {
            get
            { return numberOfSavedReports; }

            set
            {
                if (numberOfSavedReports == value)
                { return; }

                numberOfSavedReports = value;
                this.NotifyOfPropertyChange(() => this.NumberOfSavedReports);
            }
        }

        private decimal amountOfSavedReports = 0M;
        public decimal AmountOfSavedReports
        {
            get
            { return amountOfSavedReports; }

            set
            {
                if (amountOfSavedReports == value)
                { return; }

                amountOfSavedReports = value;
                this.NotifyOfPropertyChange(() => this.AmountOfSavedReports);
            }
        }

        private int numberOfPendingReports = 0;
        public int NumberOfPendingReports
        {
            get
            { return numberOfPendingReports; }

            set
            {
                if (numberOfPendingReports == value)
                { return; }

                numberOfPendingReports = value;
                this.NotifyOfPropertyChange(() => this.NumberOfPendingReports);
            }
        }

        private decimal amountOfPendingReports = 0M;
        public decimal AmountOfPendingReports
        {
            get
            { return amountOfPendingReports; }

            set
            {
                if (amountOfPendingReports == value)
                { return; }

                amountOfPendingReports = value;
                this.NotifyOfPropertyChange(() => this.AmountOfPendingReports);
            }
        }

        private int numberOfReportsNeedingApproval = 0;
        public int NumberOfReportsNeedingApproval
        {
            get
            { return numberOfReportsNeedingApproval; }

            set
            {
                if (numberOfReportsNeedingApproval == value)
                { return; }

                numberOfReportsNeedingApproval = value;
                this.NotifyOfPropertyChange(() => this.NumberOfReportsNeedingApproval);
            }
        }

        private decimal amountOfReportsNeedingApproval = 0M;
        public decimal AmountOfReportsNeedingApproval
        {
            get
            { return amountOfReportsNeedingApproval; }

            set
            {
                if (amountOfReportsNeedingApproval == value)
                { return; }

                amountOfReportsNeedingApproval = value;
                this.NotifyOfPropertyChange(() => this.AmountOfReportsNeedingApproval);
            }
        }


        #endregion


        public EmployeeViewModel EmployeeViewModel { get; protected set; }
        private SortType sortType = SortType.ItemType;
        private ObservableCollection<GroupInfoList<object>> groupedSummaryItems;
        private ObservableCollection<SummaryItemViewModel> summaryItems;
        private ObservableCollection<SummaryItemViewModel> amountMonthlyHistory;
        private ObservableCollection<SummaryItemViewModel> listOfAmountOfCharges;        
        private ObservableCollection<SummaryItemViewModel> savedReportsMonthlyHistory;
        private ObservableCollection<SummaryItemViewModel> pendingReportsMonthlyHistory;
        private ObservableCollection<SummaryItemViewModel> pastReportsMonthlyHistory;
        decimal amountOwedToCreditCard = 0M;
        decimal amountOwedToEmployee = 0M;
        private readonly IExpenseRepository _repository;
        

        private SummaryItemsViewModel() { }

        public SummaryItemsViewModel(EmployeeViewModel employeeViewModel)
        {
            this.EmployeeViewModel = employeeViewModel;
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.GroupedSummaryItems = new ObservableCollection<GroupInfoList<object>>();            
            this.SortTypes = new ObservableCollection<SortType>();
            this.SortTypes.Add(SortType.Age);
            this.SortTypes.Add(SortType.Amount);
        }

        public ObservableCollection<SummaryItemViewModel> GetSummaryItems() {
            List<SummaryItem> items = this._repository.GetSummaryItems(this.EmployeeViewModel.EmployeeId);

            var summaryItems = from item in items
                               select new SummaryItemViewModel()
                               {
                                   Id = item.Id,
                                   Amount = item.Amount,
                                   Date = Convert.ToDateTime(item.Date),
                                   Description = item.Description,
                                   Submitter = item.Submitter,
                                   ItemType = item.ItemType,
                               };
            this.SummaryItems = new ObservableCollection<SummaryItemViewModel>(summaryItems);
            this.ListOfAmountOfCharges = new ObservableCollection<SummaryItemViewModel>();
            _numberOfCharges =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge
                 select item).Count();
            _amountOfCharges =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge
                 select Convert.ToDecimal(item.Amount)).Sum();
            _numberOfChargesLT30Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days < 30
                 select item).Count();
            _amountOfChargesLT30Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days < 30
                 select Convert.ToDecimal(item.Amount)).Sum();
            _numberOfCharges30To44Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 30
                 && DateTime.Today.Subtract(item.Date).Days < 45
                 select item).Count();
            _amountOfCharges30To44Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 30
                 && DateTime.Today.Subtract(item.Date).Days < 45
                 select Convert.ToDecimal(item.Amount)).Sum();
            _numberOfChargesGT45Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 45
                 select item).Count();
            _amountOfChargesGT45Days =
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.Charge &&
                 DateTime.Today.Subtract(item.Date).Days >= 45
                 select Convert.ToDecimal(item.Amount)).Sum();

            ListOfAmountOfCharges.Add(new SummaryItemViewModel() { Amount = _amountOfChargesLT30Days, Description = "Less than 30 days" });
            ListOfAmountOfCharges.Add(new SummaryItemViewModel() { Amount = _amountOfCharges30To44Days, Description = "30-45 days"});
            ListOfAmountOfCharges.Add(new SummaryItemViewModel() { Amount = _amountOfChargesGT45Days, Description = "Over 35 days" });

            if (_numberOfCharges > 0)
            {
                _beginDateOfCharges = (from item in this.SummaryItems
                                      where item.ItemType == ItemType.Charge
                                      select item.Date).Min();
                _endDateOfCharges = (from item in this.SummaryItems
                                    where item.ItemType == ItemType.Charge
                                    select item.Date).Max();
            }

            numberOfSavedReports = (from item in this.SummaryItems
                                    where item.ItemType == ItemType.SavedReport
                                    select item).Count();
            amountOfSavedReports = Convert.ToDecimal(
                (from item in summaryItems
                 where item.ItemType == ItemType.SavedReport
                 select item.Amount).Sum());

            numberOfPendingReports = (from item in this.SummaryItems
                                      where item.ItemType == ItemType.PendingReport
                                      select item).Count();
            amountOfPendingReports = Convert.ToDecimal(
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.PendingReport
                 select item.Amount).Sum());

            var expenseReportsViewModel = new ExpenseReportsViewModel();
            amountOwedToCreditCard = this._repository.GetAmountOwedToCreditCard(this.EmployeeViewModel.EmployeeId);
            amountOwedToEmployee = this._repository.GetAmountOwedToEmployee(this.EmployeeViewModel.EmployeeId);

            numberOfReportsNeedingApproval = (from item in this.SummaryItems
                                              where item.ItemType == ItemType.UnresolvedReport
                                              select item).Count();
            amountOfReportsNeedingApproval = Convert.ToDecimal(
                (from item in this.SummaryItems
                 where item.ItemType == ItemType.UnresolvedReport
                 select item.Amount).Sum());

            List<ExpenseReport> expenseReports = this._repository.GetExpenseReports(this.EmployeeViewModel.EmployeeId);
            SavedReportsMonthlyHistory = GetSavedReportsMonthlyHistory(expenseReports);
            PendingReportsMonthlyHistory = GetPendingReportsMonthlyHistory(expenseReports);
            PastReportsMonthlyHistory = GetPastReportsMonthlyHistory(expenseReports);
            AmountMonthlyHistory = GetAmountMonthlyHistory(expenseReports);
            DateTime minDate = DateTime.Now.AddYears(-1);
            var a = expenseReports.GroupBy(i => i.Status).ToList();
            if(SavedReportsMonthlyHistory.Count != 0) {
                DateTime min = SavedReportsMonthlyHistory.Min(x=>x.Date);
                if(min < minDate)
                    minDate = min;                
            }
            if(PendingReportsMonthlyHistory.Count != 0) {
                DateTime min = PendingReportsMonthlyHistory.Min(x=>x.Date);
                if(min < minDate)
                    minDate = min;                
            }
            if(PastReportsMonthlyHistory.Count != 0) {
                DateTime min = PastReportsMonthlyHistory.Min(x=>x.Date);
                if(min < minDate)
                    minDate = min;                
            }
            BeginDateOfHistory = minDate;
            EndDateOfHistory = DateTime.Now;
            HistorySelectionStartDate = EndDateOfHistory.AddYears(-1);
            HistorySelectionEndDate = EndDateOfHistory;
            GroupItems();

            return this.SummaryItems;
        }
        ObservableCollection<SummaryItemViewModel> GetSavedReportsMonthlyHistory(List<ExpenseReport> items) {
            return new ObservableCollection<SummaryItemViewModel>(items
                                                                  .Where(i => i.Status == ExpenseReportStatus.Saved)
                                                                  .Select(i => new { Amount = i.Amount, Date = new DateTime(i.DateSaved.Value.Year, i.DateSaved.Value.Month, 1), ActualDate = i.DateSaved.Value })
                                                                  .GroupBy(i => i.Date)
                                                                  .OrderBy(i => i.Key)
                                                                  .Select(i => new SummaryItemViewModel() { Date = i.Key, Amount = i.Sum(t => t.Amount), Items = new ObservableCollection<SummaryItem>(i.Select(x => new SummaryItem() { Amount = x.Amount, Date = x.ActualDate })) })
                                                                  );
        }        
        ObservableCollection<SummaryItemViewModel> GetPendingReportsMonthlyHistory(List<ExpenseReport> items) {
            return new ObservableCollection<SummaryItemViewModel>(items
                                                                  .Where(i => i.Status == ExpenseReportStatus.Pending)
                                                                  .Select(i => new { Amount = i.Amount, Date = new DateTime(i.DateSubmitted.Value.Year, i.DateSubmitted.Value.Month, 1), ActualDate = i.DateSubmitted.Value })
                                                                  .GroupBy(i => i.Date)
                                                                  .OrderBy(i => i.Key)
                                                                  .Select(i => new SummaryItemViewModel() { Date = i.Key, Amount = i.Sum(t => t.Amount), Items = new ObservableCollection<SummaryItem>(i.Select(x => new SummaryItem() { Amount = x.Amount, Date = x.ActualDate })) })
                                                                  );
        }
        ObservableCollection<SummaryItemViewModel> GetPastReportsMonthlyHistory(List<ExpenseReport> items) {
            return new ObservableCollection<SummaryItemViewModel>(items
                                                                  .Where(i => i.Status == ExpenseReportStatus.Approved)
                                                                  .Select(i => new { Amount = i.Amount, Date = new DateTime(i.DateResolved.Value.Year, i.DateResolved.Value.Month, 1), ActualDate = i.DateResolved.Value })
                                                                  .GroupBy(i => i.Date)
                                                                  .OrderBy(i => i.Key)
                                                                  .Select(i => new SummaryItemViewModel() { Date = i.Key, Amount = i.Sum(t => t.Amount), Items = new ObservableCollection<SummaryItem>(i.Select(x => new SummaryItem() { Amount = x.Amount, Date = x.ActualDate })) })
                                                                  );
        }
        
        ObservableCollection<SummaryItemViewModel> GetAmountMonthlyHistory(List<ExpenseReport> items) {
            return new ObservableCollection<SummaryItemViewModel>(items
                                                                    .Select(i => new {
                                                                        Amount = i.Amount,
                                                                        Date = i.Status == ExpenseReportStatus.Approved ? new DateTime(i.DateResolved.Value.Year, i.DateResolved.Value.Month, 1) : (i.Status == ExpenseReportStatus.Pending ? new DateTime(i.DateSubmitted.Value.Year, i.DateSubmitted.Value.Month, 1) : new DateTime(i.DateSaved.Value.Year, i.DateSaved.Value.Month, 1)),
                                                                        ActualDate = i.Status == ExpenseReportStatus.Approved ? i.DateResolved.Value : (i.Status == ExpenseReportStatus.Pending ? i.DateSubmitted.Value : i.DateSaved.Value),
                                                                        ItemType = i.Status == ExpenseReportStatus.Approved ? ItemType.ApprovedReport : (i.Status == ExpenseReportStatus.Pending ? ItemType.PendingReport : ItemType.SavedReport),
                                                                    })
                                                                    .GroupBy(i => i.Date)
                                                                    .OrderBy(i => i.Key)
                                                                    .Select(i => new SummaryItemViewModel() { Date = i.Key, Amount = i.Sum(t => t.Amount), Items = new ObservableCollection<SummaryItem>(i.Select(x => new SummaryItem() { Amount = x.Amount, Date = x.ActualDate, ItemType = x.ItemType })) })
                                                                  );
        }
        public void GroupItems()
        {
            switch (this.SortType)
            {
                case SortType.Age:
                    GroupItemsByTypeAndAge();
                    break;
                case SortType.Amount:
                    GroupItemsByTypeAndAmount();
                    break;
                case SortType.Submitter:
                    GroupItemsBySubmitter();
                    break;
                case SortType.ItemType:
                    GroupItemsByTypeAndAge();
                    break;
                default:
                    break;
            }
        }

        private void GroupItemsByTypeAndAge()
        {
            var query = from item in summaryItems
                        orderby item.ItemType, item.Date ascending
                        group item by item.ItemType into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedSummaryItems.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                string itemTypeName = null;
                ItemType itemType = (ItemType)group.GroupName;
                switch (itemType)
                {
                    case ItemType.Charge:
                        itemTypeName = "Charges";

                        string beginMonth = _beginDateOfCharges.ToString("M").Substring(0, 3);
                        int beginDay = _beginDateOfCharges.Day;
                        int beginYear = _beginDateOfCharges.Year;
                        string endMonth = _endDateOfCharges.ToString("M").Substring(0, 3);
                        int endDay = _endDateOfCharges.Day;
                        int endYear = _endDateOfCharges.Year;

                        if (beginYear == endYear)
                        {
                            items.Details1 =
                                string.Format("{0} {1} - {2} {3}, {4}",
                                beginMonth, beginDay, endMonth, endDay, endYear);
                        }
                        else
                        {
                            items.Details1 =
                                string.Format("{0} {1}, {2} - {3} {4}, {5}",
                                beginMonth, beginDay, beginYear, endMonth, endDay, endYear);
                        }

                        items.Details2 = string.Format("{0} new for {1:C}",
                            _numberOfChargesLT30Days.ToString(),
                            _amountOfChargesLT30Days);
                        items.Details3 = string.Format("{0} old for {1:C}",
                            _numberOfCharges30To44Days.ToString(),
                            _amountOfCharges30To44Days);
                        items.Details4 = string.Format("{0} late for {1:C}",
                            _numberOfChargesGT45Days.ToString(),
                            _amountOfChargesGT45Days);
                        break;
                    case ItemType.SavedReport:
                        itemTypeName = "Saved Reports";
                        break;
                    case ItemType.PendingReport:
                        itemTypeName = "Pending Reports";
                        items.Details1 = string.Format("{0:C} owed to employee",
                            amountOwedToEmployee);
                        items.Details2 = string.Format("{0:C} owed to card",
                            amountOwedToCreditCard);
                        break;
                    case ItemType.UnresolvedReport:
                        itemTypeName = "Needing Approval";
                        break;
                    case ItemType.ApprovedReport:
                        itemTypeName = "Past Reports";
                        break;
                    default:
                        break;
                }
                items.Key = itemTypeName;
                items.Amount = System.Convert.ToDecimal(group.Amount);
                if (group.Amount == 0)
                {
                    items.Summary = "You have none";
                    items.Details1 = string.Empty;
                    items.Details2 = string.Empty;
                    items.Details3 = string.Empty;
                    items.Details4 = string.Empty;
                }
                else
                {
                    items.Summary = string.Format("{0} for {1:C}",
                        group.Count, items.Amount);
                }
                items.ImportList(group.Items.ToArray());
                if(!(itemTypeName == "Needing Approval" && !this.EmployeeViewModel.IsManager))
                {
                    this.GroupedSummaryItems.Add(items);
                }
            }
        }

        internal GroupInfoList<object> GroupItemsBySubmitter()
        {
            var query = from item in this.SummaryItems
                        orderby item.Submitter
                        group item by item.Submitter into g
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
                var items = new GroupInfoList<object>();
                if (group.Count > 0)
                {
                    items.Key = group.GroupName;
                    items.Summary = string.Format("{0} for {1:C}",
                        items.Count, items.Amount);
                }
                else
                {
                    items.Key = group.GroupName;
                }
                items.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (object item in items)
                {
                    items.Add(item);
                }
                groups.Add(items);
            }

            return groups;
        }


        private void GroupItemsByTypeAndAmount()
        {
            var query = from item in SummaryItems
                        orderby item.ItemType, item.Amount descending
                        group item by item.ItemType into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.Amount),
                            Items = g
                        };

            this.GroupedSummaryItems.Clear();
            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                string itemTypeName = null;
                ItemType itemType = (ItemType)group.GroupName;
                switch (itemType)
                {
                    case ItemType.Charge:
                        itemTypeName = "Charges";
                        break;
                    case ItemType.SavedReport:
                        itemTypeName = "Saved Reports";
                        break;
                    case ItemType.PendingReport:
                        itemTypeName = "Pending Reports";
                        break;
                    case ItemType.UnresolvedReport:
                        itemTypeName = "Needing Approval";
                        break;
                    default:
                        break;
                }
                items.Key = string.Format(
                    "{0} ({1:C})", itemTypeName, group.Amount);
                items.Amount = System.Convert.ToDecimal(group.Amount);
                items.ImportList(group.Items.ToArray());
                this.GroupedSummaryItems.Add(items);
            }
        }

    }
}
