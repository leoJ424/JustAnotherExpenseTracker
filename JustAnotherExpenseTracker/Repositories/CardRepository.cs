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
using JustAnotherExpenseTracker.Models.API_Models;
using System.Net.Http;

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

        public string ReturnCardName(Guid cardId)
        {
            string cardName = "Invalid";

            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCardName";
                    command.Parameters.Add("@CardID", System.Data.SqlDbType.UniqueIdentifier).Value = cardId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cardName = reader[0].ToString();
                        }
                    }
                }
            }

            return cardName;
        }

        public async Task<CreditCardModel> getMaskedCard_API(Guid cardId)
        {
            string url = $"api/CreditCard/cardDetails/0?cardId={cardId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    CreditCardModelAPI creditCardFromAPI = await response.Content.ReadAsAsync<CreditCardModelAPI>();

                    return convertCardFromAPI(creditCardFromAPI);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<CreditCardModel> getCard_API(Guid cardId)
        {
            string url = $"api/CreditCard/cardDetails/101?cardId={cardId}";

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    CreditCardModelAPI creditCardFromAPI = await response.Content.ReadAsAsync<CreditCardModelAPI>();

                    return convertCardFromAPI(creditCardFromAPI);
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<Guid>> getCardIdsOfCurrentUser_API()
        {
            string url = "api/CreditCard";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if(response.IsSuccessStatusCode)
                {
                    List<Guid> cardIds = await response.Content.ReadAsAsync <List<Guid>>();
                    return cardIds;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private CreditCardModel convertCardFromAPI(CreditCardModelAPI creditCardFromAPI)
        {
            return new CreditCardModel()
            {
                CardID = creditCardFromAPI.CreditCardID,
                First4Digits = creditCardFromAPI.First4Digits,
                Second4Digits = creditCardFromAPI.Second4Digits,
                Third4Digits = creditCardFromAPI.Third4Digits,
                Last4Digits = creditCardFromAPI.Last4Digits,
                CardholderName = creditCardFromAPI.CardholderName,
                ExpDate = creditCardFromAPI.ExpDate.ToString("MM/yy", CultureInfo.InvariantCulture),
                Cvc = creditCardFromAPI.CVC == 0 ? "..." : creditCardFromAPI.CVC.ToString(),
                Network = creditCardFromAPI.Network.ToString(),
                Bank = creditCardFromAPI.BankName,
                CreditLimit = creditCardFromAPI.CreditLimit,
                StatementGenDate = creditCardFromAPI.StatementGenDay,
                PaymentIn = creditCardFromAPI.PaymentDueIn,
                CardName = creditCardFromAPI.CardName,
            };
        }

        
    }
}
