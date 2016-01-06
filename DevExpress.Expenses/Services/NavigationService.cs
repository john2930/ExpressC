using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Expenses.Wpf
{
    public class NavigationService : INavigationService
    {
        public event EventHandler<EventArgs<ChargeViewModel>> ShowChargeRequested;
        public event EventHandler ShowChargesRequested;
        public event EventHandler ShowSummaryRequested;
        public event EventHandler<EventArgs<ExpenseReportViewModel>> ShowExpenseReportRequested;
        public event EventHandler ShowPendingExpenseReportsRequested;
        public event EventHandler ShowSavedExpenseReportsRequested;
        public event EventHandler ShowPastExpenseReportsRequested;
        public event EventHandler ShowReportsForApprovalRequested;
        public event EventHandler ChangeUserRequested;
        public event EventHandler CreateNewReportRequested;

        public void ChangeUser() {
            EventHandler handler = this.ChangeUserRequested;
            if(handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
        public void ShowReportsForApproval() {
            EventHandler handler = this.ShowReportsForApprovalRequested;
            if(handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
        public void ShowCharge(ChargeViewModel chargeViewModel)
        {
            EventHandler<EventArgs<ChargeViewModel>> handler = this.ShowChargeRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ChargeViewModel>(chargeViewModel));
            }
        }

        public void ShowSummary() {
            EventHandler handler = this.ShowSummaryRequested;
            if(handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
        public void ShowCharges()
        {
            EventHandler handler = this.ShowChargesRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        
        public void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel)
        {
            EventHandler<EventArgs<ExpenseReportViewModel>> handler = this.ShowExpenseReportRequested;
            if (handler != null)
            {
                handler(this, new EventArgs<ExpenseReportViewModel>(expenseReportViewModel));
            }
        }

        public void ShowPendingExpenseReports()
        {
            EventHandler handler = this.ShowPendingExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowSavedExpenseReports()
        {
            EventHandler handler = this.ShowSavedExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void ShowPastExpenseReports()
        {
            EventHandler handler = this.ShowPastExpenseReportsRequested;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void CreateNewReport() {
            EventHandler handler = this.CreateNewReportRequested;
            if(handler != null) {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
