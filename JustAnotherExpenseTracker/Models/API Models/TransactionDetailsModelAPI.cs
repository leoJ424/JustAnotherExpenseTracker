using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models.API_Models
{
    public enum TransactionModeEnum
    {
        Credit,
        Debit
    }
    public class TransactionDetailsModelAPI
    {
        public string CategoryName { get; set; }
        public string RecipientName { get; set; }
        public double Amount { get; set; }
        public TransactionModeEnum TransactionMode { get; set; }
        public double RewardPoints { get; set; }
        public DateTime Date { get; set; }
        public string? GeneralComments { get; set; }
    }
}
