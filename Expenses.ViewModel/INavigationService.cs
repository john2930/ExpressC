using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.ViewModel
{
    public interface INavigationService
    {
        void ShowCharge(ChargeViewModel chargeViewModel);
        void ShowCharges();
        void ShowExpenseReport(ExpenseReportViewModel expenseReportViewModel);
        void ShowPendingExpenseReports();
        void ShowSavedExpenseReports();
        void ShowPastExpenseReports();
        void ShowReportsForApproval();
        void ShowSummary();
        void ChangeUser();
        void CreateNewReport();
    }
}
