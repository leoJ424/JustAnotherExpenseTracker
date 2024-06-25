﻿using System;
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

        /// <summary>
        /// Returns the top (max - 4) categories where money has been spent
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <param name="CardId"></param>
        /// <returns></returns>
        List<KeyValuePair<Guid, decimal>> ReturnCardTransactionAmountsGroupByCategory(DateTime date1, DateTime date2, Guid CardId);


    }
}
