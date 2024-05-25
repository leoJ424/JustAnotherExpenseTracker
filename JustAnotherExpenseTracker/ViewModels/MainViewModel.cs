using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private CreditCardModel _creditCard;

        private bool _isShowButtonVisible = true;
        private bool _isHideButtonVisible = false;



        public UserAccountModel CurrentUserAccount 
        {
            get
            {
                return _currentUserAccount;
            }
            set 
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
         
        }

        public bool IsShowButtonVisible
        {
            get
            {
                return _isShowButtonVisible;
            }
            set
            {
                _isShowButtonVisible = value;
                OnPropertyChanged(nameof(IsShowButtonVisible));
            }
        }

        public bool IsHideButtonVisible
        {
            get
            {
                return _isHideButtonVisible;
            }
            set
            {
                _isHideButtonVisible = value;
                OnPropertyChanged(nameof(IsHideButtonVisible));
            }
        }
        public CreditCardModel CreditCard 
        { 
            get
            { 
                return _creditCard; 
            } 
            set
            { 
                _creditCard = value; 
                OnPropertyChanged(nameof(CreditCard));
            } 
        }
        private void createCreditCard()
        {
            CreditCard = new CreditCardModel()
            {
                First4Digits = "1 3 0 9",
                Second4Digits = "3 4 9 9",
                Third4Digits = "7 2 5 3",
                Last4Digits = "5 5 4 5",
                CardholderName = "Jane Doe",
                ExpDate = "01/28",
                Cvc = "123",
                Balance = "$19,521",
            };
        }

        private void createMaskedCreditCard()
        {
            CreditCard = new CreditCardModel()
            {
                First4Digits = ". . . .",
                Second4Digits = ". . . .",
                Third4Digits = ". . . .",
                Last4Digits = "5 5 4 5",
                CardholderName = "Jane Doe",
                ExpDate = "01/28",
                Cvc = "...",
                Balance = "$19,521",

            };
        }
        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            //LoadCurrentUserData();
            createMaskedCreditCard(); //TBD
            HideCardDetailsCommand = new ViewModelCommand(ExecuteHideCardDetailsCommand);
            ShowCardDetailsCommand = new ViewModelCommand(ExecuteShowCardDetailsCommand);
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Welcome {user.Name} {user.LastName}.";
                CurrentUserAccount.ProfilePicture = null;
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid User, not logged in";
            }
        }

        //-> Commands
        public ICommand HideCardDetailsCommand { get; }
        public ICommand ShowCardDetailsCommand { get; }
        

        private void ExecuteHideCardDetailsCommand(object obj)
        {
            createMaskedCreditCard();
            OnPropertyChanged(nameof(CreditCard));

            IsShowButtonVisible = true; 
            OnPropertyChanged(nameof(IsShowButtonVisible));
            IsHideButtonVisible = false;
            OnPropertyChanged(nameof(IsHideButtonVisible));

        }

        private void ExecuteShowCardDetailsCommand(object obj)
        {
            createCreditCard();
            OnPropertyChanged(nameof(CreditCard));

            IsShowButtonVisible = false;
            OnPropertyChanged(nameof(IsShowButtonVisible));
            IsHideButtonVisible = true;
            OnPropertyChanged(nameof(IsHideButtonVisible));
        }
    }
}
