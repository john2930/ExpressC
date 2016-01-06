using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Model
{
    public class ExpenseReport
    {
        public decimal Amount { get; set; }
        public string Approver { get; set; }
        public int CostCenter { get; set; }
        public Nullable<DateTime> DateResolved { get; set; }
        public Nullable<DateTime> DateSaved { get; set; }
        public Nullable<DateTime> DateSubmitted { get; set; }
        public int EmployeeId { get; set; }
        public int ExpenseReportId { get; set; }
        public decimal OwedToCreditCard { get; set; }
        public decimal OwedToEmployee { get; set; }
        public string Notes { get; set; }
        public string Purpose { get; set; }
        public ExpenseReportStatus Status { get; set; }
    }

}
