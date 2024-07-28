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
        List<KeyValuePair<DateTime, decimal>> ReturnCardDebitTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId);
        List<KeyValuePair<DateTime, decimal>> ReturnCardCreditTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId);
        List<KeyValuePair<int, decimal>> ReturnCardDebitTransactionAmountsGroupByMonth(DateTime date1, DateTime date2, Guid CardId);
        List<KeyValuePair<int, decimal>> ReturnCardCreditTransactionAmountsGroupByMonth(DateTime date1, DateTime date2, Guid CardId);
        DateTime ReturnEarliestTransactionDateOnCard(Guid CardID);
        DateTime ReturnLatestTransactionDateOnCard(Guid CardID);

        /// <summary>
        /// Returns the top (max - 4) categories where money has been spent
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="CardId"></param>
        /// <returns></returns>
        List<KeyValuePair<Guid, decimal>> ReturnCardTransactionAmountsGroupByCategory(DateTime date1, DateTime date2, Guid CardId);

        /// <summary>
        /// Returns the data to be displayed for the Detailed Transactions View
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="CardId"></param>
        /// <returns></returns>
        List<TransactionDetailsCustomModel> ReturnTransactionDetailsForView(DateTime date1, DateTime date2, Guid CardId);


    }
}
