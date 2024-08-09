using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models.API_Models
{
    public enum NetworkEnum
    {
        VISA,
        MasterCard,
        American_Express,
        Discover
    }
    public class CreditCardModelAPI
    {
        public Guid CreditCardID { get; set; }
        public string First4Digits { get; set; }
        public string Second4Digits { get; set; }
        public string Third4Digits { get; set; }
        public string Last4Digits { get; set; }
        public string CardName { get; set; }
        public string CardholderName { get; set; }
        public NetworkEnum Network { get; set; }
        public string BankName { get; set; }
        public DateTime ExpDate { get; set; }
        public int CVC { get; set; }
        public double CreditLimit { get; set; }
        public int StatementGenDay { get; set; }
        public int PaymentDueIn { get; set; }
    }
}
