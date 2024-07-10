using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Services;
using JustAnotherExpenseTracker.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class DetailedTransactionsViewModel : ViewModelBase
    {
        private INavigationService _navigation;

        private DateTime _startDate;
        private DateTime _endDate;
        private Guid _currentCard;
        private string _startDateDisplay;
        private string _endDateDisplay;


        private List<Guid> cards;
        

        public DetailedTransactionsViewModel(INavigationService navService)
        {
            Navigation = navService;

            StartDate = Navigation.PassedDataForDetailedTransactionsView.StartDate;
            EndDate = Navigation.PassedDataForDetailedTransactionsView.EndDate; 
            CurrentCard = Navigation.PassedDataForDetailedTransactionsView.CurrentCard;
            cards = Navigation.PassedDataForDetailedTransactionsView.CardIDs;

            StartDateDisplay = StartDate.ToString("dd-MMM-yyyy");
            EndDateDisplay = EndDate.ToString("dd-MMM-yyyy");

            GetDetailedTransactionDataCommand = new ViewModelCommand(ExecuteGetDetailedTransactionDataCommand);
        }

        #region Properties
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

        public DateTime StartDate 
        {
            get 
            { 
                return _startDate; 
            } 
            set 
            { 
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        public DateTime EndDate 
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public Guid CurrentCard
        {
            get
            {
                return _currentCard;
            }
            set
            {
                _currentCard = value;
                OnPropertyChanged(nameof(CurrentCard));
            }
        }

        public string StartDateDisplay
        {
            get
            {
                return _startDateDisplay;
            }
            set
            {
                _startDateDisplay = value;
                OnPropertyChanged(nameof(StartDateDisplay));
            }
        }

        public string EndDateDisplay
        {
            get
            {
                return _endDateDisplay;
            }
            set
            {
                _endDateDisplay = value;
                OnPropertyChanged(nameof(EndDateDisplay));
            }
        }

        #endregion

        #region Commands
        public ICommand GetDetailedTransactionDataCommand { get; }
        #endregion
        private void ExecuteGetDetailedTransactionDataCommand(object obj)
        {
            var temp = Navigation.CurrentView;
            var temp1 = StartDate;
            var temp2 = EndDate;
        }
    }
}
