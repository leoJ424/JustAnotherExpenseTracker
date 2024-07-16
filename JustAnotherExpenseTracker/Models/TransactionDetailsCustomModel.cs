using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public class TransactionDetailsCustomModel
    {
        public string CategoryName { get; set; }
        public string RecipientName { get; set; }
        public string Amount { get; set; }
        public string TransactionType { get; set; }
        public double RewardPoints { get; set; }
        public string DateOfTransaction { get; set; }
        public string GeneralComments { get; set; }
    }
}
