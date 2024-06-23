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

        public List<KeyValuePair<DateTime, decimal>> ReturnCardTransactionAmountsGroupByDate(DateTime date1, DateTime date2, Guid CardId)
        {
            var returnList = new List<KeyValuePair<DateTime, decimal>>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getTransactionDetailsBetweenDatesBasedOnCardIDGroupByDate";
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
    }
}
