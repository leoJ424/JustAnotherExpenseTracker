using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using JustAnotherExpenseTracker.Services;
using JustAnotherExpenseTracker.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class DetailedTransactionsViewModel : ViewModelBase
    {
        private INavigationService _navigation;

        private ICardRepository cardRepository;
        private ITransactionRepository transactionRepository;

        private DateTime _startDate;
        private DateTime _endDate;
        private Guid _currentCard;
        private string _startDateDisplay;
        private string _endDateDisplay;
        private string _cardName;
        private List<Guid> _cards;
        private bool _noDataDisplay = false;

        public DetailedTransactionsViewModel(INavigationService navService)
        {
            Navigation = navService;
            cardRepository = new CardRepository();
            transactionRepository = new TransactionRepository();

            StartDate = Navigation.PassedDataForDetailedTransactionsView.StartDate;
            EndDate = Navigation.PassedDataForDetailedTransactionsView.EndDate; 
            CurrentCard = Navigation.PassedDataForDetailedTransactionsView.CurrentCard;
            Cards = Navigation.PassedDataForDetailedTransactionsView.CardIDs;

            StartDateDisplay = StartDate.ToString("dd-MMM-yyyy");
            EndDateDisplay = EndDate.ToString("dd-MMM-yyyy");

            CardName = cardRepository.ReturnCardName(CurrentCard);

            CardNamesForComboBox = new ObservableCollection<string>();
            DetailsOfTransactions = new ObservableCollection<TransactionDetailRowViewModel>();

            GetDetailedTransactionDataCommand = new ViewModelAsyncCommand(ExecuteGetDetailedTransactionDataCommand);

            //Getting the card names to be displayed in the combo box
            GetCardNames();
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

        public List<Guid> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
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

        public string CardName
        {
            get
            {
                return _cardName;
            }
            set
            {
                _cardName = value;
                OnPropertyChanged(nameof(CardName));
            }
        }

        public bool NoDataDisplay
        {
            get
            {
                return _noDataDisplay;
            }
            set
            {
                _noDataDisplay = value;
                OnPropertyChanged(nameof(NoDataDisplay));
            }
        }

        #endregion

        #region Custom Properties
        public ObservableCollection<string> CardNamesForComboBox { get; set; }
        public ObservableCollection<TransactionDetailRowViewModel> DetailsOfTransactions { get; set; }

        #endregion

        #region Commands
        public ICommand GetDetailedTransactionDataCommand { get; }
        #endregion
        private async Task ExecuteGetDetailedTransactionDataCommand(object obj)
        {
            DetailsOfTransactions.Clear();
            var dataFromDB = await transactionRepository.GetTransactionDetailsForView(StartDate, EndDate, CurrentCard);
            foreach (var row in dataFromDB)
            {
                //For the usercontrol
                DetailsOfTransactions.Add(new TransactionDetailRowViewModel
                {
                    CategoryName = row.CategoryName,
                    RecipientName = row.RecipientName,
                    Amount = row.Amount,
                    TransactionType = row.TransactionType,
                    RewardPoints = row.RewardPoints,
                    DateOfTransaction = row.DateOfTransaction,
                    GeneralComments = row.GeneralComments,
                });
            }

            if(DetailsOfTransactions.Count == 0) // No Transaction Data
            {
                NoDataDisplay = true;
            }
            else
            {
                NoDataDisplay = false;
            }
        }

        private void GetCardNames()
        {
            foreach (var cardID in Cards)
            {
                CardNamesForComboBox.Add(cardRepository.ReturnCardName(cardID));
            }
        }
    }
}
