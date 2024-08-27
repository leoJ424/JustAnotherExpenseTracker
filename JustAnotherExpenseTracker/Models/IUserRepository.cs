using JustAnotherExpenseTracker.Models.API_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
        Task<UserModel_API> GetCurrentUserData_API();

    }
}
