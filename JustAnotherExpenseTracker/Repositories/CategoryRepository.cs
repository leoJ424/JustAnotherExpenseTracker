using JustAnotherExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public string GetCategoryName(Guid CategoryID)
        {
            string categoryName = string.Empty;

            using (var connection = GetConnection())
            {
                connection.Open();
                using(var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;

                    command.CommandText = "sp_getCategoryName";
                    command.Parameters.Add("@CategoryID", System.Data.SqlDbType.UniqueIdentifier).Value = CategoryID;

                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoryName = reader.GetString(0);
                        }
                    }
                }
            }
            return categoryName;

        }

        public async Task<string> GetCategoryName_API(Guid CategoryID)
        {
            string url = $"api/Category/{CategoryID}";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if(response.IsSuccessStatusCode)
                {
                    string categoryName = await response.Content.ReadAsAsync<string>();
                    return categoryName;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
