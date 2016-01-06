using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Model
{
    public class Charge
    {
        public int AccountNumber { get; set; }
        public decimal BilledAmount { get; set; }
        public int ChargeId { get; set; }
        public CategoryType Category { get; set; }
        public string Description { get; set; }
        public int EmployeeId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Nullable<int> ExpenseReportId { get; set; }
        public ChargeType ExpenseType { get; set; }
        public string Location { get; set; }
        public string Merchant { get; set; }
        public string Notes { get; set; }
        public bool ReceiptRequired { get; set; }
        public decimal TransactionAmount { get; set; }
    }
}
