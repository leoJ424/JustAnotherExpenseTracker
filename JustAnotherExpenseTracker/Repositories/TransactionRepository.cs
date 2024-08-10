using JustAnotherExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using JustAnotherExpenseTracker.Models.API_Models;
using System.Net.Http;
using ExpenseTrackerWebAPI_Mk2.Dto;

namespace JustAnotherExpenseTracker.Repositories
{
    public class TransactionRepository : RepositoryBase, ITransactionRepository
    {

        public TransactionDetailsModel ReturnTransactionDetais(Guid TransactionId)
        {
            TransactionDetailsModel transactionDetails = new TransactionDetailsModel();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getTransactionDetailsByID";
                    command.Parameters.Add("@TransactionID", System.Data.SqlDbType.UniqueIdentifier).Value = TransactionId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var guid = reader.GetGuid(0);
                            transactionDetails.TransactionID = (Guid)reader[0];
                            transactionDetails.CategoryID = (Guid)reader[2];
                            transactionDetails.RecipientID = (Guid)reader[3];
                            transactionDetails.BankID = (Guid)reader[4];
                            transactionDetails.CardID = (Guid)reader[5];
                            transactionDetails.PaymentMethod = reader[6].ToString();
                            transactionDetails.TransactionType = reader[7].ToString();    
                            transactionDetails.Amount = (decimal)reader[8];    
                            transactionDetails.RewardPoints = (decimal)reader[9];    
                            transactionDetails.TransactionDate = Convert.ToDateTime(reader[10]);    
                            transactionDetails.GeneralComments = reader[11].ToString();    
                        }
                    }
                }
            }

            return transactionDetails;
        }

        public List<Guid> ReturnTransactionIDsBasedOnBankIDs(DateTime date1, DateTime date2, Guid BankId)
        {
            throw new NotImplementedException();
        }

        public List<Guid> ReturnTransactionIDsBasedOnCardIDs(DateTime date1, DateTime date2, Guid CardId)
        {
            List<Guid> transactionIds = new List<Guid>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getTransactionDetailsBetweenDatesBasedOnCardID";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var guid = reader.GetGuid(0);
                            transactionIds.Add(guid);
                        }
                    }
                }
            }

            return transactionIds;
        }

        public List<KeyValuePair<DateTime, decimal>> ReturnCardDebitTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<DateTime, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getDebitTransactionDetailsBetweenDatesBasedOnCardIDGroupByDate";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnList.Add(new KeyValuePair<DateTime, decimal>(Convert.ToDateTime(reader[1]), Convert.ToDecimal(reader[0])));
                        }
                    }
                }
            }

            return returnList;

        }

        public List<KeyValuePair<DateTime, decimal>> ReturnCardCreditTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<DateTime, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCreditTransactionDetailsBetweenDatesBasedOnCardIDGroupByDate";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnList.Add(new KeyValuePair<DateTime, decimal>(Convert.ToDateTime(reader[1]), Convert.ToDecimal(reader[0])));
                        }
                    }
                }
            }

            return returnList;

        }

        public List<KeyValuePair<int, decimal>> ReturnCardDebitTransactionAmountsGroupByMonth(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<int, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getDebitTransactionDetailsBetweenDatesBasedOnCardIDGroupByMonth";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnList.Add(new KeyValuePair<int, decimal>(Convert.ToInt32(reader[1]), Convert.ToDecimal(reader[0])));
                        }
                    }
                }
            }

            return returnList;

        }

        public List<KeyValuePair<int, decimal>> ReturnCardCreditTransactionAmountsGroupByMonth(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<int, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCreditTransactionDetailsBetweenDatesBasedOnCardIDGroupByMonth";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnList.Add(new KeyValuePair<int, decimal>(Convert.ToInt32(reader[1]), Convert.ToDecimal(reader[0])));
                        }
                    }
                }
            }

            return returnList;

        }

        public DateTime ReturnEarliestTransactionDateOnCard(Guid CardID)
        {
            var earliestDate = new DateTime();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getEarliestTransactionDateonCard";
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardID;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            earliestDate = Convert.ToDateTime(reader[0]);
                        }
                    }
                }
            }
            return earliestDate;
        }

        public DateTime ReturnLatestTransactionDateOnCard(Guid CardID)
        {
            var latestDate = new DateTime();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getLatestTransactionDateonCard";
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardID;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            latestDate = Convert.ToDateTime(reader[0]);
                        }
                    }
                }
            }
            return latestDate;
        }

        public List<KeyValuePair<Guid, decimal>> ReturnCardTransactionAmountsGroupByCategory(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<Guid, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getTransactionDetailsBetweenDatesBasedOnCardIDGroupByCategory";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnList.Add(new KeyValuePair<Guid, decimal>(reader.GetGuid(1), Convert.ToDecimal(reader[0])));
                        }
                    }
                }
            }

            return returnList;

        }

        public List<TransactionDetailsCustomModel> ReturnTransactionDetailsForView(DateTime date1, DateTime date2, Guid CardId)
        {
            List<TransactionDetailsCustomModel> transactionDetails = new List<TransactionDetailsCustomModel>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getTransactionDetailsForCardsView";
                    command.Parameters.Add("@Date1", System.Data.SqlDbType.Date).Value = date1;
                    command.Parameters.Add("@Date2", System.Data.SqlDbType.Date).Value = date2;
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = CardId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TransactionDetailsCustomModel tempDetails = new TransactionDetailsCustomModel()
                            {
                                CategoryName = reader[0].ToString(),
                                RecipientName = reader[1].ToString(),
                                Amount = Convert.ToDouble(reader[2]).ToString("C", CultureInfo.GetCultureInfo("en-us")),
                                TransactionType = reader[3].ToString(),
                                RewardPoints = Convert.ToDouble(reader[4]),
                                DateOfTransaction = Convert.ToDateTime(reader[5]).ToString("dd-MMM-yy"),
                                GeneralComments = reader[6].ToString(),
                            };
                            transactionDetails.Add(tempDetails);
                        }
                    }
                }
            }

            return transactionDetails;
        }

        //APIs
        public async Task<DateTime> GetEarliestTransactionDateOnCard_API(Guid CardID)
        {
            string url = $"api/Transaction/TransactionDetailsEarliestDate?CreditCardId={CardID}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DateTime earliestTransactionDateFromAPI = await response.Content.ReadAsAsync<DateTime>();

                    return earliestTransactionDateFromAPI;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<DateTime> GetLatestTransactionDateOnCard_API(Guid CardID)
        {
            string url = $"api/Transaction/TransactionDetailsLatestDate?CreditCardId={CardID}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    DateTime latestTransactionDateFromAPI = await response.Content.ReadAsAsync<DateTime>();

                    return latestTransactionDateFromAPI;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionDate_AmountPairs>> GetCardDebitTransactionAmountsGroupByDate_API(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/Debit_TransactionDetails_Datewise?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using(HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if(response.IsSuccessStatusCode)
                {
                    List<TransactionDate_AmountPairs> transactionsDate_AmountPairs = await response.Content.ReadAsAsync<List<TransactionDate_AmountPairs>>();
                    return transactionsDate_AmountPairs;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionDate_AmountPairs>> GetCardCreditTransactionAmountsGroupByDate_API(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/Credit_TransactionDetails_Datewise?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionDate_AmountPairs> transactionsDate_AmountPairs = await response.Content.ReadAsAsync<List<TransactionDate_AmountPairs>>();
                    return transactionsDate_AmountPairs;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionCategory_AmountPairs>> GetCardTransactionAmountsGroupByCategory_API(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/TransactionDetailsByCategory?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionCategory_AmountPairs> transactionCategory_AmountPairs = await response.Content.ReadAsAsync<List<TransactionCategory_AmountPairs>>();
                    return transactionCategory_AmountPairs;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionMonth_AmountPairs>> GetCardDebitTransactionAmountsGroupByMonth_API(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/Debit_TransactionDetails_Monthwise?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionMonth_AmountPairs> transactionMonth_AmountPairs = await response.Content.ReadAsAsync<List<TransactionMonth_AmountPairs>>();
                    return transactionMonth_AmountPairs;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionMonth_AmountPairs>> GetCardCreditTransactionAmountsGroupByMonth_API(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/Credit_TransactionDetails_Monthwise?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionMonth_AmountPairs> transactionMonth_AmountPairs = await response.Content.ReadAsAsync<List<TransactionMonth_AmountPairs>>();
                    return transactionMonth_AmountPairs;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<TransactionDetailsCustomModel>> GetTransactionDetailsForView(DateTime date1, DateTime date2, Guid CardId)
        {
            var newDate1 = date1.ToString("yyyy-MM-dd");
            var newDate2 = date2.ToString("yyyy-MM-dd");

            string url = $"api/Transaction/TransactionDetailsDetailed?date1={newDate1}&date2={newDate2}&CreditCardId={CardId}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<TransactionDetailsModelAPI> transactionDetails = await response.Content.ReadAsAsync<List<TransactionDetailsModelAPI>>();
                    return convertTransactionDetailsFromAPI(transactionDetails);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private List<TransactionDetailsCustomModel> convertTransactionDetailsFromAPI(List<TransactionDetailsModelAPI> transactionDetailsAPI)
        {
            List<TransactionDetailsCustomModel> transactionDetails = new List<TransactionDetailsCustomModel>();
            foreach(var transaction_API in  transactionDetailsAPI)
            {
                TransactionDetailsCustomModel transaction = new TransactionDetailsCustomModel
                {
                    CategoryName = transaction_API.CategoryName,
                    RecipientName = transaction_API.RecipientName,
                    Amount = transaction_API.Amount.ToString("F2"),
                    TransactionType = transaction_API.TransactionMode.ToString(),
                    RewardPoints = transaction_API.RewardPoints,
                    DateOfTransaction = transaction_API.Date.ToString("dd-MMM-yy"),
                    GeneralComments = transaction_API.GeneralComments
                };
                transactionDetails.Add(transaction);
            }
            return transactionDetails;
        }

    }
}
