using FontAwesome.Sharp;
using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using JustAnotherExpenseTracker.Services;
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
        private INavigationService _navigation;

        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;

        private string _caption;
        private IconChar _icon;
        private SolidColorBrush _captionColor;

        private bool _isBtnOpenUserOptionsVisible = true;

        //Properties
        public INavigationService Navigation
        {
            get
            {
                return _navigation;
            }
            set
            {
                _navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }

        }

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

        /// <summary>
        /// Visibility control for the buttons that can open and close the user options
        /// </summary>
        public bool IsBtnOpenUserOptionsVisible
        {
            get
            {
                return _isBtnOpenUserOptionsVisible;
            }
            set
            {
                _isBtnOpenUserOptionsVisible = value;
                OnPropertyChanged(nameof(IsBtnOpenUserOptionsVisible));
            }
        }

        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            userRepository = new UserRepository();
            ApiHelper.InitializeClient();

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
            ShowStocksViewCommand = new ViewModelCommand(ExecuteShowStocksViewCommand);
            ShowBanksViewCommand = new ViewModelCommand(ExecuteShowBanksViewCommand);
            ShowDashboardViewCommand = new ViewModelCommand(ExecuteShowDashboardViewCommand);
            OpenUserOptionsCommand = new ViewModelCommand(ExecuteOpenUserOptionsCommand);
            CloseUserOptionsCommand = new ViewModelCommand(ExecuteCloseUserOptionsCommand);
        }

        private void ExecuteShowCardsViewCommand(object obj)
        {
            if(CurrentUserAccount.CreditCards.Count == 0)
            {
                Navigation.NavigateTo<CardsNotAvailableViewModel>();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
                CaptionColor = (SolidColorBrush) obj;
            }
            else
            {
                Navigation.NavigateTo<CardsViewModel>();
                Caption = "Cards";
                Icon = IconChar.CreditCard;
                CaptionColor = (SolidColorBrush)obj;
            }
        }

        private void ExecuteShowStocksViewCommand(object obj)
        {
            Navigation.NavigateTo<StocksViewModel>();
            Caption = "Stocks";
            Icon = IconChar.MagnifyingGlassChart;
            CaptionColor = (SolidColorBrush)obj;           
        }

        private void ExecuteShowBanksViewCommand(object obj)
        {
            Navigation.NavigateTo<BanksViewModel>();
            Caption = "Banks";
            Icon = IconChar.BuildingColumns;
            CaptionColor = (SolidColorBrush)obj;
        }

        private void ExecuteShowDashboardViewCommand(object obj)
        {
            Navigation.NavigateTo<DashboardViewModel>();
            Caption = "Dashboard";
            Icon = IconChar.Home;
            CaptionColor = (SolidColorBrush)obj;
        }

        private void ExecuteOpenUserOptionsCommand(object obj)
        {
            IsBtnOpenUserOptionsVisible = false;
        }

        private void ExecuteCloseUserOptionsCommand(object obj)
        {
            IsBtnOpenUserOptionsVisible = true;
        }

        //-> Commands
        public ICommand ShowCardsViewCommand { get; }
        public ICommand ShowStocksViewCommand { get; }
        public ICommand ShowBanksViewCommand { get; }
        public ICommand ShowDashboardViewCommand { get; }
        public ICommand OpenUserOptionsCommand { get; }
        public ICommand CloseUserOptionsCommand { get; }
        public ICommand LogOutCommand { get; }
    }
}
