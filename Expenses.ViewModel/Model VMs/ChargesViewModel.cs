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
    public class ChargesViewModel : ViewModelBase
    {
        #region Properties

        public int NumberLT30Days
        {
            get
            { return this._numberLT30Days; }

            set
            {
                if (this._numberLT30Days == value) { return; }

                this._numberLT30Days = value;
                this.NotifyOfPropertyChange(() => this.NumberLT30Days);
            }
        }
        private int _numberLT30Days = 0;

        public decimal AmountLT30Days
        {
            get { return this._amountLT30Days; }
            set
            {
                if (this._amountLT30Days == value) { return; }

                this._amountLT30Days = value;
                this.NotifyOfPropertyChange(() => this.AmountLT30Days);
            }
        }
        private decimal _amountLT30Days = 0M;

        public int Number30To44Days
        {
            get { return this._number30To44Days; }
            set
            {
                if (this._number30To44Days == value) { return; }

                this._number30To44Days = value;
                this.NotifyOfPropertyChange(() => this.Number30To44Days);
            }
        }
        private int _number30To44Days = 0;

        public decimal Amount30To44Days
        {
            get { return this._amount30To44Days; }
            set
            {
                if (this._amount30To44Days == value) { return; }

                this._amount30To44Days = value;
                this.NotifyOfPropertyChange(() => this.Amount30To44Days);
            }
        }
        private decimal _amount30To44Days = 0M;

        public int NumberGT45Days
        {
            get { return this._numberGT45Days; }
            set
            {
                if (this._numberGT45Days == value) { return; }

                this._numberGT45Days = value;
                this.NotifyOfPropertyChange(() => this.NumberGT45Days);
            }
        }
        private int _numberGT45Days = 0;

        public decimal AmountGT45Days
        {
            get { return this._amountGT45Days; }
            set
            {
                if (this._amountGT45Days == value) { return; }

                this._amountGT45Days = value;
                this.NotifyOfPropertyChange(() => this.AmountGT45Days);
            }
        }
        private decimal _amountGT45Days = 0M;

        public DateTime BeginDate
        {
            get { return this._beginDate; }
            set
            {
                if (this._beginDate == value) { return; }

                this._beginDate = value;
                this.NotifyOfPropertyChange(() => this.BeginDate);
            }
        }
        private DateTime _beginDate = DateTime.Today;

        public DateTime SelectionBeginDate {
            get { return this._SelectionbeginDate; }
            set {
                if(this._SelectionbeginDate == value) { return; }

                this._SelectionbeginDate = value;
                this.NotifyOfPropertyChange(() => this.SelectionBeginDate);
                List<ChargeViewModel> chargesWhereToList = Charges.Where(c => c.ExpenseDate >= SelectionBeginDate && c.ExpenseDate <= SelectionEndDate).ToList();
                FilteredCharges = Charges == null ? null : new ObservableCollection<ChargeViewModel>(chargesWhereToList);
            }
        }
        private DateTime _SelectionbeginDate = DateTime.Today;

        public DateTime EndDate
        {
            get { return this._endDate; }
            set
            {
                if (this._endDate == value) { return; }

                this._endDate = value;
                this.NotifyOfPropertyChange(() => this.EndDate);
            }
        }
        private DateTime _endDate = DateTime.Today;

        public DateTime SelectionEndDate {
            get { return this._SelectionendDate; }
            set {
                if(this._SelectionendDate == value) { return; }

                this._SelectionendDate = value;
                this.NotifyOfPropertyChange(() => this.SelectionEndDate);
                List<ChargeViewModel> chargesWhereToList = Charges.Where(c => c.ExpenseDate >= SelectionBeginDate && c.ExpenseDate <= SelectionEndDate).ToList();
                FilteredCharges = Charges == null ? null : new ObservableCollection<ChargeViewModel>(chargesWhereToList);
            }
        }
        private DateTime _SelectionendDate = DateTime.Today;

        public ObservableCollection<SortType> SortTypes { get; set; }

        public ObservableCollection<GroupInfoList<object>> GroupedCharges
        {
            get { return _groupedCharges; }
            set
            {
                this._groupedCharges = value;
                this.NotifyOfPropertyChange(() => this.GroupedCharges);
            }
        }
        private ObservableCollection<GroupInfoList<object>> _groupedCharges;

        public ObservableCollection<ChargeViewModel> Charges
        {
            get { return this._charges; }
            set
            {
                this._charges = value;
                this.NotifyOfPropertyChange(() => this.Charges);
            }
        }
        private ObservableCollection<ChargeViewModel> _charges;

        public ObservableCollection<ChargeViewModel> FilteredCharges {
            get { return this._Filteredcharges; }
            set {
                this._Filteredcharges = value;
                this.NotifyOfPropertyChange(() => this.FilteredCharges);
            }
        }
        private ObservableCollection<ChargeViewModel> _Filteredcharges;
        public SortType SortType
        {
            get { return this._sortType; }
            set
            {
                if (this._sortType == value)
                {
                    return;
                }

                this._sortType = value;
                this.NotifyOfPropertyChange(() => this.SortType);

                //update the grouping because the sort/filter type changed
                GroupItems();
            }
        }
        private SortType _sortType = SortType.ItemType;

        #endregion

        public ICommand ViewChargeCommand { get; private set; }
        public ICommand ChargesSelectionChangedCommand { get; private set; }
        public ICommand NewChargeCommand { get; private set; }

        private List<ChargeViewModel> _resultsSelectedItems = new List<ChargeViewModel>();


        public IExpenseRepository Repository
        {
            get { return _repository; }
        }
        
        
        private readonly IExpenseRepository _repository;

        private readonly INavigationService NavigationService;
        private readonly EmployeeViewModel EmployeeViewModel;
        private readonly IViewService ViewService;

        public ChargesViewModel()
        {
            this.EmployeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();
            this.GroupedCharges = new ObservableCollection<GroupInfoList<object>>();
            this.SortTypes = new ObservableCollection<SortType>();
            this.SortTypes.Add(SortType.Age);
            this.SortTypes.Add(SortType.Amount);

            this.ViewChargeCommand = new RelayCommand(
                (charge) =>
                {
                    this.ViewCharge(charge as ChargeViewModel);
                });

            this.ChargesSelectionChangedCommand = new RelayCommand(
                (selectionChange) =>
                {
                    this.ChargesSelectionChanged(selectionChange);
                });
            this.NewChargeCommand = new RelayCommand(x => CreateNewCharge());
        }

        public void ChargesSelectionChanged(object selectedItems)
        {

        }

        public void LoadCharges() {
            List<Charge> outstandingCharges = this._repository.GetOutstandingCharges(this.EmployeeViewModel.EmployeeId);
            LoadChargesHelper(outstandingCharges);
            SelectionBeginDate = BeginDate = outstandingCharges.Select(c => c.ExpenseDate).Min();
            SelectionEndDate = EndDate = outstandingCharges.Select(c => c.ExpenseDate).Max();
        }

        public void LoadCharges(int expenseReportId) {            
            List<Charge> reportCharges = this._repository.GetCharges(expenseReportId);
            LoadChargesHelper(reportCharges);
        }

        public void SaveCharges() {            
            foreach (var c in this.Charges)
                c.Save();            
        }

        private void LoadChargesHelper(List<Charge> chargeList)
        {
            var charges = from charge in chargeList
                          select new ChargeViewModel()
                          {
                              Charge = charge,
                          };
            this.Charges = new ObservableCollection<ChargeViewModel>(charges);
            FilteredCharges = Charges;

            foreach (ChargeViewModel charge in this.Charges)
            {
                if (DateTime.Today.Subtract(charge.ExpenseDate).Days >= 45)
                {
                    charge.ForegroundColor = "#FFD33131";
                }
                else
                {
                    charge.ForegroundColor = "Black";
                }
            }

            this.NumberLT30Days = (from charge in charges
                                   where DateTime.Today.Subtract(charge.ExpenseDate).Days < 30
                                   select charge.BilledAmount).Count();
            this.AmountLT30Days = (from charge in charges
                                   where DateTime.Today.Subtract(charge.ExpenseDate).Days < 30
                                   select charge.BilledAmount).Sum();
            this.Number30To44Days = (from charge in charges
                                     where DateTime.Today.Subtract(charge.ExpenseDate).Days >= 30
                                           && DateTime.Today.Subtract(charge.ExpenseDate).Days < 45
                                     select charge.BilledAmount).Count();
            this.Amount30To44Days = (from charge in charges
                                     where DateTime.Today.Subtract(charge.ExpenseDate).Days >= 30
                                           && DateTime.Today.Subtract(charge.ExpenseDate).Days < 45
                                     select charge.BilledAmount).Sum();
            this.NumberGT45Days = (from charge in charges
                                   where DateTime.Today.Subtract(charge.ExpenseDate).Days >= 45
                                   select charge.BilledAmount).Count();
            this.AmountGT45Days = (from charge in charges
                                   where DateTime.Today.Subtract(charge.ExpenseDate).Days >= 45
                                   select charge.BilledAmount).Sum();

            if (charges.Count() > 0)
            {
                //this._beginDate = (from charge in charges
                //                   select charge.ExpenseDate).Min();

                //this._endDate = (from charge in charges
                //                 select charge.ExpenseDate).Max();
                this.BeginDate = (from charge in charges
                                   select charge.ExpenseDate).Min();

                this.EndDate = (from charge in charges
                                 select charge.ExpenseDate).Max();
            }
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

        internal GroupInfoList<object> GroupItemsBySubmitter()
        {
            var query = from item in Charges
                        orderby item.EmployeeId
                        group item by item.EmployeeId into g
                        select new
                        {
                            GroupName = g.Key,
                            Count = g.Count(),
                            Amount = g.Sum(e => e.TransactionAmount),
                            Items = g
                        };

            var groups = new GroupInfoList<object>();
            foreach (var group in query)
            {
                var items = new GroupInfoList<object>();
                items.Key = string.Format(
                    "{0} ({1:C})", group.GroupName, group.Amount);
                items.Amount = System.Convert.ToDecimal(group.Amount);
                foreach (object item in items)
                {
                    items.Add(item);
                }
                groups.Add(items);
            }

            return groups;
        }

        private void GroupItemsByTypeAndAge()
        {
        }

        private void GroupItemsByTypeAndAmount()
        {
        }

        public event EventHandler CreateNewChargeRequested;
        
        void CreateNewCharge() {
            ChargeViewModel viewModel = new ChargeViewModel() {
                EmployeeId = EmployeeViewModel.EmployeeId
            };
            if(CreateNewChargeRequested != null) {
                CreateNewChargeRequested(viewModel, EventArgs.Empty);
            }
        }

        public event EventHandler ViewChargeRequested;

        private void ViewCharge(ChargeViewModel chargeViewModel)
        {
            if(ViewChargeRequested != null)
                ViewChargeRequested(chargeViewModel, EventArgs.Empty);
        }
    }
}
