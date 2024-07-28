using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public class TransactionDetailsModel
    {
        public Guid TransactionID { get; set; }
        public Guid UserID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid RecipientID { get; set; }
        public Guid BankID { get; set; }
        public Guid CardID { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal RewardPoints { get; set; }
        public DateTime TransactionDate { get; set; }
        public string GeneralComments { get; set; }





    }
}
