using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public interface IExpenseRepository
    {
        Employee GetEmployee(string employeeAlias);
        int SaveEmployee(Employee employee);
        List<SummaryItem> GetSummaryItems(int employeeId);
        ExpenseReport GetExpenseReport(int expenseReportId);
        List<ExpenseReport> GetExpenseReports(int employeeId);
        List<ExpenseReport> GetExpenseReportsByStatus(int employeeId, ExpenseReportStatus status);
        List<ExpenseReport> GetExpenseReportsForApproval(string employeeAlias);
        List<Charge> GetCharges(int expenseReportId);
        Charge GetCharge(int chargeId);
        List<Charge> GetOutstandingCharges(int employeeId);
        decimal GetAmountOwedToCreditCard(int employeeId);
        decimal GetAmountOwedToEmployee(int employeeId);
        int SaveCharge(Charge charge);
        void DeleteCharge(int chargeId);
        int SaveExpenseReport(ExpenseReport expenseReport);
        void DeleteExpenseReport(int expenseReportId);
        void ResetData();
    }
}
