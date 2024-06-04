using FontAwesome.Sharp;
using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using JustAnotherExpenseTracker.Views.UserControlsForMainView;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private ICardRepository cardRepository;
        private CreditCardModel _creditCard;


        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private List<Guid> cards;

        //Properties
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

        /// <summary>
        /// Tells us which childview is to be shown on the main view
        /// </summary>

        public ViewModelBase CurrentChildView 
        { 
            get 
            {
                return _currentChildView; 
            } 
            set 
            {
                _currentChildView = value; 
                OnPropertyChanged(nameof(CurrentChildView));
            } 
        }

        /// <summary>
        /// The title for when a child view is displayed
        /// </summary>
        public string Caption 
        { 
            get
            { 
                return _caption; 
            }
            set
            {
                _caption = value; 
                OnPropertyChanged(nameof(Caption));
            } 
        }

        /// <summary>
        /// Icon to go along with the above caption/title
        /// </summary>
        public IconChar Icon 
        { 
            get 
            {
                return _icon; 
            }
            set 
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
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

            CurrentUserAccount = new UserAccountModel();
            cardRepository = new CardRepository();
            LoadCurrentUserData();
            ShowCardsViewCommand = new ViewModelCommand(ExecuteShowCardsViewCommand);
            //createMaskedCreditCard(); //TBD

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
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid User, not logged in";
            }
        }

        private void ExecuteShowCardsViewCommand(object obj)
        {
            if(cards.Count == 0)
            {
                CurrentChildView = new CardsNotAvailableViewModel();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
            }
            else
            {
                CurrentChildView = new CardsViewModel();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
            }
        }

        //-> Commands
        public ICommand ShowCardsViewCommand { get; }
    }
}
