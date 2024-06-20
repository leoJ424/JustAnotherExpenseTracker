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
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;

        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;
        private SolidColorBrush _captionColor;

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

        /// <summary>
        /// Color to go along with the above caption/title
        /// </summary>
        public SolidColorBrush CaptionColor
        {
            get
            {
                return _captionColor;
            }
            set
            {
                _captionColor = value;
                OnPropertyChanged(nameof(CaptionColor));
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
            CurrentUserAccount = LoadCurrentUserData(Thread.CurrentPrincipal.Identity.Name);
            ShowCardsViewCommand = new ViewModelCommand(ExecuteShowCardsViewCommand);
        }

        private void ExecuteShowCardsViewCommand(object obj)
        {
            if(CurrentUserAccount.CreditCards.Count == 0)
            {
                CurrentChildView = new CardsNotAvailableViewModel();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
                CaptionColor = (SolidColorBrush) obj;
            }
            else
            {
                CurrentChildView = new CardsViewModel();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
                CaptionColor = (SolidColorBrush)obj;
            }
        }

        //-> Commands
        public ICommand ShowCardsViewCommand { get; }
    }
}
