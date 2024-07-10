using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public class TransactionDetailsCustomModel
    {
        public string CategoryName;
        public string RecipientName;
        public double Amount;
        public string TransactionType;
        public double RewardPoints;
        public DateTime DateOfTransaction;
        public string GeneralComments;
    }
}
