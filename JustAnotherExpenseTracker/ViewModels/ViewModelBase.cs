using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace JustAnotherExpenseTracker.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private UserAccountModel _currentUserAccount = new UserAccountModel();

        private IUserRepository userRepository = new UserRepository();
        private ICardRepository cardRepository = new CardRepository();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected UserAccountModel LoadCurrentUserData(string username)
        {
            var user = userRepository.GetByUsername(username);

            if (user != null)
            {
                _currentUserAccount.Username = user.Username;
                _currentUserAccount.DisplayName = $"{user.Name} {user.LastName}";
                _currentUserAccount.ProfilePicture = null;

                _currentUserAccount.CreditCards = cardRepository.ReturnCardIDsofUser(new NetworkCredential(user.Username, user.Password));
                
            }
            else
            {
                _currentUserAccount.DisplayName = "Invalid User, not logged in";
            }

            return _currentUserAccount;
        }
    }
}
