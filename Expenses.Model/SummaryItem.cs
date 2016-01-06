using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Model
{
    public class SummaryItem
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public string Submitter { get; set; }
        public ItemType ItemType { get; set; }
    }
}
