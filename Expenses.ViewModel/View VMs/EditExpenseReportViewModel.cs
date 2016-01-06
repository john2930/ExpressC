using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class EditExpenseReportViewModel : ViewModelBase
    {
        public bool CanSave
        {
            get { return _canSave; }
            set 
            {
                if (this._canSave == value) { return; }

                this._canSave = value;
                this.NotifyOfPropertyChange(() => this.CanSave);
            }
        }
        private bool _canSave = true;

        public bool CanSubmit
        {
            get { return this._canSubmit; }
            set
            {
                if (this._canSubmit == value) { return; }

                _canSubmit = value;
                this.NotifyOfPropertyChange(() => this.CanSubmit);
            }
        }
        private bool _canSubmit = true;

        public bool CanApprove {
            get { return this._canApprove; }
            set {
                if(this._canApprove == value) { return; }

                _canApprove = value;
                this.NotifyOfPropertyChange(() => this.CanApprove);
            }
        }
        private bool _canApprove = true;

        public bool CanDelete
        {
            get { return _canDelete; }
            set
            {
                if (this._canDelete == value) { return; }

                this._canDelete = value;
                this.NotifyOfPropertyChange(() => this.CanDelete);
            }
        }
        private bool _canDelete = true;

        public bool CanModifyCharges
        {
            get { return _canModifyCharges; }
            set 
            {
                if (this._canModifyCharges == value) { return; }

                this._canModifyCharges = value;
                this.NotifyOfPropertyChange(() => this.CanModifyCharges);
            }
        }
        private bool _canModifyCharges;

        public bool IsNewReport
        {
            get { return this._isNewReport; }
            set 
            {
                if (this._isNewReport == value) { return; }

                this._isNewReport = value;
                this.NotifyOfPropertyChange(() => this.IsNewReport);
            }
        }
        private bool _isNewReport = true;

        public ExpenseReportViewModel ExpenseReport
        {
            get { return _expenseReportViewModel; }
            set
            {
                _expenseReportViewModel = value;
                this.UpdateBindableProperties();
            }
        }
        private ExpenseReportViewModel _expenseReportViewModel;

        public AddChargesViewModel AddCharges
        {
            get { return _addChargesViewModel; }
            set { _addChargesViewModel = value; }
        }
        private AddChargesViewModel _addChargesViewModel;

        public ExpenseReportChargesViewModel AssociatedCharges
        {
            get { return _expenseReportChargesView; }
            set { _expenseReportChargesView = value; }
        }
        private ExpenseReportChargesViewModel _expenseReportChargesView;

        public ICommand AddChargeToReportCommand { get; private set; }
        public ICommand RemoveChargeFromReportCommand { get; private set; }
        public ICommand SaveReportCommand { get; private set; }

        private readonly IViewService ViewService;

        public EditExpenseReportViewModel()
        {
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            this.AddChargeToReportCommand = 
                new RelayCommand(
                    (charge) =>
                    {
                        this.AddChargeToReport(charge as ChargeViewModel);
                    });

            this.RemoveChargeFromReportCommand = 
                new RelayCommand(
                    (charge) =>
                    {
                        this.RemoveChargeFromReport(charge as ChargeViewModel);
                    });

            this.SaveReportCommand = 
                new RelayCommand(
                    (report) =>
                    {
                        // TODO: Figure out out these were originally calculated.
                        this._expenseReportViewModel.Amount = this._expenseReportChargesView.Charges.Sum(item => item.TransactionAmount);
                        this._expenseReportViewModel.OwedToCreditCard = this._expenseReportChargesView.Charges.Sum(item => item.TransactionAmount);
                        this._expenseReportViewModel.OwedToEmployee = 0;
                        
                        this._expenseReportViewModel.Save();
                        this._addChargesViewModel.SaveCharges();
                        this._expenseReportChargesView.SaveCharges();
                        
                        this.UpdateBindableProperties();
                    });
        }

        private void UpdateBindableProperties()
        {
            this.SetCanSaveHelper();
            this.SetIsNewReportHelper();
            this.SetCanModifyChargesHelper();
            this.SetCanSubmitHelper();
            this.SetCanApproveHelper();
            this.SetCanDeleteHelper();
        }

        private void SetCanDeleteHelper()
        {
            this.CanDelete = (this._expenseReportViewModel.Status == Model.ExpenseReportStatus.Saved) && !this.IsNewReport;
        }

        private void SetCanSubmitHelper()
        {
            this.CanSubmit = (this._expenseReportViewModel.Status == Model.ExpenseReportStatus.Saved) && !this.IsNewReport;
        }

        private void SetCanApproveHelper() {
            this.CanApprove = (this._expenseReportViewModel.Status == Model.ExpenseReportStatus.Pending) && !this.IsNewReport;
        }

        private void SetCanModifyChargesHelper()
        {
            this.CanModifyCharges = this.CanSave && !this.IsNewReport;
        }

        private void SetCanSaveHelper()
        {
            this.CanSave = (this._expenseReportViewModel.Status == Model.ExpenseReportStatus.Saved) || (this._expenseReportViewModel.ExpenseReportId == 0);
        }
        private void SetIsNewReportHelper()
        {
            this.IsNewReport = (this._expenseReportViewModel.ExpenseReportId == 0);
        }

        private void AddChargeToReport(ChargeViewModel chargeViewModel)
        {
            this._addChargesViewModel.Charges.Remove(chargeViewModel);

            chargeViewModel.ExpenseReportId = this.ExpenseReport.ExpenseReportId;
            this._expenseReportChargesView.Charges.Add(chargeViewModel);
        }

        private void RemoveChargeFromReport(ChargeViewModel chargeViewModel)
        {
            chargeViewModel.ExpenseReportId = null;
            this._expenseReportChargesView.Charges.Remove(chargeViewModel);
            this._addChargesViewModel.Charges.Add(chargeViewModel);
        }
    }
}
