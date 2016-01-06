﻿//*********************************************************
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expenses.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class ExpenseReportViewModel : ViewModelBase
    {
        #region Properties
        public ExpenseReportsViewModel ExpenseReportsViewModel { get; protected set; }
        private int _expenseReportId = 0;
        public int ExpenseReportId
        {
            get
            { return _expenseReportId; }

            set
            {
                if (_expenseReportId == value)
                { return; }

                _expenseReportId = value;
                this.NotifyOfPropertyChange(() => this.ExpenseReportId);
            }
        }

        private int employeeId = 0;
        public int EmployeeId
        {
            get
            { return employeeId; }

            set
            {
                if (employeeId == value)
                { return; }

                employeeId = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.EmployeeId);
            }
        }

        private string employeeName = string.Empty;

        private decimal amount = 0M;
        public decimal Amount
        {
            get
            { return amount; }

            set
            {
                if (amount == value)
                { return; }

                amount = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Amount);
            }
        }

        private string purpose = string.Empty;
        public string Purpose
        {
            get
            { return purpose; }

            set
            {
                if (purpose == value)
                { return; }

                purpose = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Purpose);
            }
        }

        private string approver = string.Empty;
        public string Approver
        {
            get
            { return approver; }

            set
            {
                if (approver == value)
                { return; }

                approver = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Approver);
            }
        }

        private int costCenter = 0;
        public int CostCenter
        {
            get
            { return costCenter; }

            set
            {
                if (costCenter == value)
                { return; }

                costCenter = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.CostCenter);
            }
        }

        private string notes = string.Empty;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                if (notes == value)
                { return; }

                notes = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.Notes);
            }
        }

        private DateTime? dateSaved = null;
        public DateTime? DateSaved
        {
            get
            {
                return dateSaved;
            }

            set
            {
                if (dateSaved == value)
                { return; }

                dateSaved = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.DateSaved);
            }
        }

        private DateTime? dateSubmitted = null;
        public DateTime? DateSubmitted
        {
            get
            {
                return dateSubmitted;
            }

            set
            {
                if (dateSubmitted == value)
                { return; }

                dateSubmitted = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.DateSubmitted);
            }
        }

        private ExpenseReportStatus status = ExpenseReportStatus.Saved;
        public ExpenseReportStatus Status
        {
            get
            {
                return status;
            }

            set
            {
                if (status == value)
                { return; }

                status = value;
                isDirty = true;

                this.NotifyOfPropertyChange(() => this.Status);
            }
        }

        private DateTime? dateResolved = null;
        public DateTime? DateResolved
        {
            get
            {
                return dateResolved;
            }

            set
            {
                if (dateResolved == value)
                { return; }

                dateResolved = value;
                isDirty = true;
                this.NotifyOfPropertyChange(() => this.DateResolved);
            }
        }

        private decimal owedToCreditCard = 0M;
        public decimal OwedToCreditCard
        {
            get
            {
                return owedToCreditCard;
            }

            set
            {
                if (owedToCreditCard == value)
                { return; }

                owedToCreditCard = value;
                this.NotifyOfPropertyChange(() => this.OwedToCreditCard);
            }
        }

        private decimal owedToEmployee = 0M;
        public decimal OwedToEmployee
        {
            get
            {
                return owedToEmployee;
            }

            set
            {
                if (owedToEmployee == value)
                { return; }

                owedToEmployee = value;
                this.NotifyOfPropertyChange(() => this.OwedToEmployee);
            }
        }

        private string backgroundColor;
        public string BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                backgroundColor = value;
                this.NotifyOfPropertyChange(() => this.BackgroundColor);
            }
        }

        private string foregroundColor;
        public string ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                foregroundColor = value;
                this.NotifyOfPropertyChange(() => this.ForegroundColor);
            }
        }

        private bool isDirty = false;
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }

            set
            {
                isDirty = value;
                this.NotifyOfPropertyChange(() => this.IsDirty);
            }
        }

        public DateTime DisplayDate
        {
            get { return this._displayDate; }
            set
            {
                this._displayDate = value;
                this.NotifyOfPropertyChange(() => this.DisplayDate);
            }
        }
        private DateTime _displayDate;

        #endregion "Properties"

        private readonly IExpenseRepository _repository;
        private readonly INavigationService NavigationService;
        private readonly IViewService ViewService;
        private readonly EmployeeViewModel _employeeViewModel;


        public ICommand SaveReportCommand { get; private set; }
        public ICommand DeleteReportCommand { get; private set; }
        public ICommand ApproveReportCommand { get; private set; }
        public ICommand SubmitReportCommand { get; private set; }

        ExpenseReportChargesViewModel associatedCharges;
        public ExpenseReportChargesViewModel AssociatedCharges { 
            get {
                if(associatedCharges == null) {
                    CreateAndLoadCharges();
                }
                return associatedCharges;
            } 
        }
        void CreateAndLoadCharges() {
            associatedCharges = new ExpenseReportChargesViewModel();
            associatedCharges.LoadCharges(ExpenseReportId);
        }

        public ExpenseReportViewModel()
        {
            this._repository = ServiceLocator.Current.GetService<IExpenseRepository>();
            this._employeeViewModel = ServiceLocator.Current.GetService<EmployeeViewModel>();
            this.NavigationService = ServiceLocator.Current.GetService<INavigationService>();
            this.ViewService = ServiceLocator.Current.GetService<IViewService>();

            employeeId = this._employeeViewModel.EmployeeId;
            employeeName = this._employeeViewModel.Name;
            approver = this._employeeViewModel.Manager;
            costCenter = 50992;
            purpose = string.Empty;

            this.SaveReportCommand = new RelayCommand((_)=>this.Save());
            this.DeleteReportCommand = new RelayCommand((_) =>this.Delete());

            this.SubmitReportCommand = new RelayCommand((_) => this.Submit());
            this.ApproveReportCommand = new RelayCommand((_) => this.Approve());
        }

        internal ExpenseReportViewModel(ExpenseReportsViewModel expenseReportsViewModel, ExpenseReport expenseReport)
            : this()
        {
            ExpenseReportsViewModel = expenseReportsViewModel;
            this.Load(expenseReport);
        }


        public void Load(int expenseReportId) {
            this.Load(this._repository.GetExpenseReport(expenseReportId));
        }

        private void Load(ExpenseReport expenseReport)
        {
            this.Amount = expenseReport.Amount;
            this.Approver = expenseReport.Approver;
            this.CostCenter = expenseReport.CostCenter;
            this.DateResolved = expenseReport.DateResolved;
            this.DateSaved = expenseReport.DateSaved;
            this.DateSubmitted = expenseReport.DateSubmitted;
            this.EmployeeId = expenseReport.EmployeeId;
            this.ExpenseReportId = expenseReport.ExpenseReportId;
            this.Notes = expenseReport.Notes;
            this.OwedToCreditCard = expenseReport.OwedToCreditCard;
            this.OwedToEmployee = expenseReport.OwedToEmployee;
            this.Purpose = expenseReport.Purpose;
            this.Status = expenseReport.Status;

            switch (this.Status)
            {
                case ExpenseReportStatus.Saved: this.DisplayDate = this.DateSaved.GetValueOrDefault(); break;
                case ExpenseReportStatus.Pending: this.DisplayDate = this.DateSubmitted.GetValueOrDefault(); break;
                case ExpenseReportStatus.Approved: this.DisplayDate = this.DateResolved.GetValueOrDefault(); break;
                case ExpenseReportStatus.Canceled: this.DisplayDate = this.DateResolved.GetValueOrDefault(); break;
                case ExpenseReportStatus.Rejected: this.DisplayDate = this.DateResolved.GetValueOrDefault(); break;
            }
        }

        public void Save()
        {
            // TODO: We should change how this works. Right now it's just ported to use the new
            // API, although the workflow needs to be fixed.

            
            ExpenseReport expenseReport = new ExpenseReport();
            expenseReport.ExpenseReportId = ExpenseReportId;
            expenseReport.Amount = this.Amount;
            expenseReport.Approver = this.Approver;
            expenseReport.CostCenter = this.CostCenter;
            expenseReport.DateResolved = this.DateResolved;
            expenseReport.DateSaved = DateTime.Now;
            expenseReport.DateSubmitted = this.DateSubmitted;
            expenseReport.EmployeeId = this.EmployeeId;
            expenseReport.ExpenseReportId = this.ExpenseReportId;
            expenseReport.Notes = this.Notes;
            expenseReport.OwedToCreditCard = this.OwedToCreditCard;
            expenseReport.OwedToEmployee = this.OwedToEmployee;
            expenseReport.Purpose = this.Purpose;
            expenseReport.Status = this.Status;

            int reportId = this._repository.SaveExpenseReport(expenseReport);
            this.Load(reportId);
        }

        public void Delete() {
            this._repository.DeleteExpenseReport(this.ExpenseReportId);
            this.NavigationService.ShowSavedExpenseReports();
        }

        public void Submit()
        {
            this.Status = ExpenseReportStatus.Pending;
            this.DateSubmitted = DateTime.Now;
            this.Save();
            this.NavigationService.ShowPendingExpenseReports();
        }

        public void Approve()
        {
            //if(await this.ViewService.ConfirmAsync("Confirm expense report", "Are you sure you want to approve this expense report?")) {
                this.Status = ExpenseReportStatus.Approved;
                this.DateResolved = DateTime.Now;
                this.Save();
            //}
        }

        public void Reject(int reportNumber)
        {
            this.Status = ExpenseReportStatus.Rejected;
            this.DateResolved = DateTime.Now;
            this.Save();
        }

    }

}
