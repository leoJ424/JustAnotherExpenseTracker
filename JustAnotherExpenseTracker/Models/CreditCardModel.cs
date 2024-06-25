using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public class CreditCardModel
    {
        public Guid CardID { get; set; }
        public string First4Digits { get; set; }
        public string Second4Digits {  get; set; }
        public string Third4Digits {  get; set; }
        public string Last4Digits {  get; set; }
        public string CardholderName {  get; set; }
        public string ExpDate {  get; set; }
        public string Cvc {  get; set; }
        public string Network { get; set; }
        public string Bank { get; set; }
        public double CreditLimit { get; set; }
        public int StatementGenDate { get; set; }
        public int PaymentIn { get; set; }

    }
}
