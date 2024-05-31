using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ICardRepository cardRepository;
        private CreditCardModel _creditCard;

        private bool _isShowButtonVisible = true;
        private bool _isHideButtonVisible = false;
        private bool _isCardNextButtonVisible = false; //By Default false
        private bool _isCardPreviousButtonVisible = false; //By Default false

        private int currentCardBeingViewed = 0; // by default user views his/her first card itself

        private List<Guid> cards;

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

        /// <summary>
        /// Enabled only if there are multiple cards for the user
        /// </summary>
        public bool IsCardNextButtonVisible 
        { 
            get 
            {
                return _isCardNextButtonVisible; 
            } 
            set 
            {
                _isCardNextButtonVisible = value;
                OnPropertyChanged(nameof(IsCardNextButtonVisible));
            } 
        }

        /// <summary>
        /// Enabled if there are multiple cards for the user and the user is currently viewing any card other than the first one
        /// </summary>
        public bool IsCardPreviousButtonVisible 
        { 
            get 
            {
                return _isCardPreviousButtonVisible; 
            }
            set 
            { 
                _isCardPreviousButtonVisible = value; 
                OnPropertyChanged(nameof(IsCardPreviousButtonVisible));
            } 
        }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            //TO BE DELETED - Implemented to just make it work without logging in each time

            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential("admin", "admin"));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), null);
            }

            //END

            
            cardRepository = new CardRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
            //createMaskedCreditCard(); //TBD

            HideCardDetailsCommand = new ViewModelCommand(ExecuteHideCardDetailsCommand);
            ShowCardDetailsCommand = new ViewModelCommand(ExecuteShowCardDetailsCommand);

            ShowNextCardCommand = new ViewModelCommand(ExecuteShowNextCardCommand);
            ShowPreviousCardCommand = new ViewModelCommand(ExecuteShowPreviousCardCommand);
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"Welcome {user.Name} {user.LastName}";
                CurrentUserAccount.ProfilePicture = null;

                cards = cardRepository.ReturnCardIDsofUser(new NetworkCredential(user.Username, user.Password));
                if(cards.Count > 1)
                {
                    IsCardNextButtonVisible = true;
                    OnPropertyChanged(nameof(IsCardNextButtonVisible));
                }
                displayMaskedCard(cards[currentCardBeingViewed]);
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid User, not logged in";
            }
        }

        //-> Commands
        public ICommand HideCardDetailsCommand { get; }
        public ICommand ShowCardDetailsCommand { get; }
        public ICommand ShowNextCardCommand { get; }
        public ICommand ShowPreviousCardCommand { get; }

        private void ExecuteHideCardDetailsCommand(object obj)
        {
            //createMaskedCreditCard();
            displayMaskedCard(cards[currentCardBeingViewed]);
            //OnPropertyChanged(nameof(CreditCard));

            IsShowButtonVisible = true; 
            //OnPropertyChanged(nameof(IsShowButtonVisible));
            IsHideButtonVisible = false;
            //OnPropertyChanged(nameof(IsHideButtonVisible));

        }

        private void ExecuteShowCardDetailsCommand(object obj)
        {
            //createCreditCard();
            displayCard(cards[currentCardBeingViewed]);
            //OnPropertyChanged(nameof(CreditCard));

            IsShowButtonVisible = false;
            //OnPropertyChanged(nameof(IsShowButtonVisible));
            IsHideButtonVisible = true;
            //OnPropertyChanged(nameof(IsHideButtonVisible));
        }

        private void ExecuteShowNextCardCommand(object obj)
        {

            if(currentCardBeingViewed + 1 < cards.Count)
            {
                ++currentCardBeingViewed;
                displayMaskedCard(cards[currentCardBeingViewed]);
            }
            if(currentCardBeingViewed == cards.Count - 1)
            {
                IsCardNextButtonVisible = false;
            }
            if(currentCardBeingViewed > 0)
            {
                IsCardPreviousButtonVisible = true;
            }
        }

        private void ExecuteShowPreviousCardCommand(object obj)
        {

            if (currentCardBeingViewed - 1 >= 0)
            {
                --currentCardBeingViewed;
                displayMaskedCard(cards[currentCardBeingViewed]);
            }
            if (currentCardBeingViewed == 0)
            {
                IsCardPreviousButtonVisible = false;
            }
            if (cards.Count > 1 && currentCardBeingViewed != cards.Count - 1)
            {
                IsCardNextButtonVisible = true;
            }
        }

        //-> Functions User Defined

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

        private void displayCard(Guid id)
        {
            CreditCard = cardRepository.ReturnCardDetails(id);
        }

        private void displayMaskedCard(Guid id)
        {
            CreditCard = cardRepository.ReturnMaskedCardDetails(id);
        }
    }
}
