using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public interface ITransactionRepository
    {
        List<Guid> ReturnTransactionIDsBasedOnCardIDs(DateTime date1, DateTime date2, Guid CardId);
        List<Guid> ReturnTransactionIDsBasedOnBankIDs(DateTime date1, DateTime date2, Guid BankId);
        TransactionDetailsModel ReturnTransactionDetais(Guid TransactionId);
        List<KeyValuePair<DateTime, decimal>> ReturnCardTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId);
        DateTime ReturnEarliestTransactionDateOnCard(Guid CardID);
        DateTime ReturnLatestTransactionDateOnCard(Guid CardID);


    }
}
