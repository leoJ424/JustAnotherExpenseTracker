using JustAnotherExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;

namespace JustAnotherExpenseTracker.Repositories
{
    public class CardRepository : RepositoryBase, ICardRepository
    {

        /// <summary>
        /// Given the Username and Password, the function returns the list of all cards belonging to the user.
        /// </summary>
        public List<Guid> ReturnCardIDsofUser(NetworkCredential credential)
        {
            List<Guid> cardIDs = new List<Guid>();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCardsofUser";
                    command.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = credential.UserName;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = credential.Password;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var guid = reader.GetGuid(0);
                            cardIDs.Add(guid);
                        }
                    }
                }
            }

            return cardIDs;
        }

        public CreditCardModel ReturnCardDetails(Guid cardId)
        {
            CreditCardModel cardDetails = new CreditCardModel();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCardDetails";
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = cardId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardDetails.CardID = reader.GetGuid(0);
                            cardDetails.First4Digits = reader[2].ToString();
                            cardDetails.Second4Digits = reader[3].ToString();
                            cardDetails.Third4Digits = reader[4].ToString();
                            cardDetails.Last4Digits = reader[5].ToString();
                            cardDetails.CardholderName = reader[6].ToString();
                            cardDetails.Network = reader[7].ToString();
                            cardDetails.Bank = reader[8].ToString();
                            cardDetails.ExpDate = Convert.ToDateTime(reader[9]).ToString("MM/yy", CultureInfo.InvariantCulture);
                            cardDetails.Cvc = reader[10].ToString();
                            cardDetails.CreditLimit = Convert.ToDouble(reader[11]);
                            cardDetails.StatementGenDate = (int)reader[12];
                            cardDetails.PaymentIn = (int)reader[13];
                            cardDetails.CardName = reader[14].ToString();
                        }
                    }
                }
            }

            return cardDetails;
        }

        public CreditCardModel ReturnMaskedCardDetails(Guid cardId)
        {
            CreditCardModel cardDetails = new CreditCardModel();

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCardDetails";
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = cardId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardDetails.CardID = reader.GetGuid(0);
                            cardDetails.First4Digits = ". . . .";
                            cardDetails.Second4Digits = ". . . .";
                            cardDetails.Third4Digits = ". . . .";
                            cardDetails.Last4Digits = reader[5].ToString();
                            cardDetails.CardholderName = reader[6].ToString();
                            cardDetails.Network = reader[7].ToString();
                            cardDetails.Bank = reader[8].ToString();
                            cardDetails.ExpDate = Convert.ToDateTime(reader[9]).ToString("MM/yy", CultureInfo.InvariantCulture);
                            cardDetails.Cvc = "...";
                            cardDetails.CreditLimit = Convert.ToDouble(reader[11]);
                            cardDetails.StatementGenDate = (int)reader[12];
                            cardDetails.PaymentIn = (int)reader[13];
                            cardDetails.CardName = reader[14].ToString();
                        }
                    }
                }
            }

            return cardDetails;
        }
    }
}
