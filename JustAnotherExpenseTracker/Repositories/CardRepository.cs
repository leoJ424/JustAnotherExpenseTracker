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
    }
}
