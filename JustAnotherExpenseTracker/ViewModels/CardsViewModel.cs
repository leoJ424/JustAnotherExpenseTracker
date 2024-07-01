using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Repositories;
using LiveCharts.Defaults;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts.Wpf;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class CardsViewModel:ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private ICardRepository cardRepository;
        private ITransactionRepository transactionRepository;
        private ICategoryRepository categoryRepository;
        private CreditCardModel _creditCard;

        private bool _isShowButtonVisible = true;
        private bool _isHideButtonVisible = false;
        private bool _isCardNextButtonVisible = false; //By Default false
        private bool _isCardPreviousButtonVisible = false; //By Default false

        private string _cardDisplayAmount;
        private string _cardDisplayText;
        
        private bool _isMonthlyButtonChecked = true;//By Default true
        private bool _isYearlyButtonChecked = false;//By Default False
        private bool _isNextStatementButtonVisible = true; //By Default false
        private bool _isPreviousStatementButtonVisible = true; //By Default false

        private string _statementTextToBeDisplayed;

        private ChartValues<DateTimePoint> _seriesValues;

        private ZoomingOptions _zoomingMode;

        private List<double> _doughnutChartValues;
        private List<string> _doughnutChartCategoryNames;
        private double _totalAmountSpentOnCard;


        private int currentCardBeingViewed = 0; // by default user views his/her first card itself

        private DateTime firstStatementDate1; //To get transaction data;
        private DateTime firstStatementDate2;

        List<Tuple<DateTime, DateTime>> statementDates;

        private DateTime statementDate1;
        private DateTime statementDate2;

        private DateTime earliestTransactionDate;
        private DateTime latestTransactionDate;

        private int currentStatementView;

        private int timesClickedOnZoomToggle;

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }


        #region Properties

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
        /// Amount to be shown on the credit card. When masked shows the credit limit, when unmasked shows the balance available
        /// </summary>
        public string CardDisplayAmount
        {
            get
            {
                return _cardDisplayAmount;
            }
            set
            {
                _cardDisplayAmount = value;
                OnPropertyChanged(nameof(CardDisplayAmount));
            }
        }

        /// <summary>
        /// Text indicating what amount is being displayed. 'Credit Limit' or 'Available Balance'
        /// </summary>
        public string CardDisplayText
        {
            get
            {
                return _cardDisplayText;
            }
            set
            {
                _cardDisplayText = value;
                OnPropertyChanged(nameof(CardDisplayText));
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

        /// <summary>
        /// To show the credit card usage data in a statement month
        /// </summary>
        public bool IsMonthlyButtonChecked
        {
            get
            {
                return _isMonthlyButtonChecked;
            }
            set
            {
                _isMonthlyButtonChecked = value;
                OnPropertyChanged(nameof(IsMonthlyButtonChecked));
            }
        }

        /// <summary>
        /// To show the credit card usage data in a year
        /// </summary>
        public bool IsYearlyButtonChecked
        {
            get
            {
                return _isYearlyButtonChecked;
            }
            set
            {
                _isYearlyButtonChecked = value;
                OnPropertyChanged(nameof(IsYearlyButtonChecked));
            }
        }

        /// <summary>
        /// To show the credit card usage data in a different statement months, years, etc, set based on available data
        /// </summary>
        public bool IsNextStatementButtonVisible
        {
            get
            {
                return _isNextStatementButtonVisible;
            }
            set
            {
                _isNextStatementButtonVisible = value;
                OnPropertyChanged(nameof(IsNextStatementButtonVisible));
            }
        }

        /// <summary>
        /// To show the credit card usage data in a different statement months, years, etc, set based on available data
        /// </summary>
        public bool IsPreviousStatementButtonVisible
        {
            get
            {
                return _isPreviousStatementButtonVisible;
            }
            set
            {
                _isPreviousStatementButtonVisible = value;
                OnPropertyChanged(nameof(IsPreviousStatementButtonVisible));
            }
        }

        /// <summary>
        /// Text to show which statement is being printed. Eg. Statement for June 2024 or Entire usage for year 2023 etc etc
        /// </summary>
        public string StatementTextToBeDisplayed
        {
            get
            {
                return _statementTextToBeDisplayed;
            }
            set
            {
                _statementTextToBeDisplayed = value;
                OnPropertyChanged(nameof(StatementTextToBeDisplayed));
            }
        }

        public ChartValues<DateTimePoint> SeriesValues
        {
            get
            {
                return _seriesValues;
            }
            set
            {
                _seriesValues = value;
                OnPropertyChanged(nameof(SeriesValues));
            }
        }

        public List<double> DoughnutChartValues
        {
            get
            {
                return _doughnutChartValues;
            }
            set
            {
                _doughnutChartValues = value;
                OnPropertyChanged(nameof(DoughnutChartValues));
            }
        }

        public List<string> DoughnutChartCategoryNames
        {
            get
            {
                return _doughnutChartCategoryNames;
            }
            set
            {
                _doughnutChartCategoryNames = value;
                OnPropertyChanged(nameof(DoughnutChartCategoryNames));
            }
        }

        public double TotalAmounntSpentOnCard
        {
            get
            {
                return _totalAmountSpentOnCard;
            }
            set
            {
                _totalAmountSpentOnCard = value;
                OnPropertyChanged(nameof(TotalAmounntSpentOnCard));
            }
        }

        public ZoomingOptions ZoomingMode
        {
            get { return _zoomingMode; }
            set
            {
                _zoomingMode = value;
                OnPropertyChanged(nameof(ZoomingMode));
            }
        }

        #endregion

        public CardsViewModel()
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
            statementDates = new List<Tuple<DateTime, DateTime>>();

            timesClickedOnZoomToggle = 0; // Based on the number of times this button is clicked we set a particular zoom mode

            CurrentUserAccount = LoadCurrentUserData(Thread.CurrentPrincipal.Identity.Name);
            if(CurrentUserAccount.CreditCards.Count > 1)
            {
                IsCardNextButtonVisible = true;
            }
            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);

            HideCardDetailsCommand = new ViewModelCommand(ExecuteHideCardDetailsCommand);
            ShowCardDetailsCommand = new ViewModelCommand(ExecuteShowCardDetailsCommand);

            ShowNextCardCommand = new ViewModelCommand(ExecuteShowNextCardCommand);
            ShowPreviousCardCommand = new ViewModelCommand(ExecuteShowPreviousCardCommand);

            transactionRepository = new TransactionRepository();
            categoryRepository = new CategoryRepository();
            fillPreRequisiteData();

            generateDataForGraphDaywise(statementDates[currentStatementView].Item1, statementDates[currentStatementView].Item2, CreditCard.CardID);

            ShowNextCardStatementCommand = new ViewModelCommand(ExecuteShowNextCardStatementCommand); ;
            ShowPreviousCardStatementCommand = new ViewModelCommand(ExecuteShowPreviousCardStatementCommand);
            ToggleZoomModeForGraphCommand = new ViewModelCommand(ExecuteToggleZoomModeForGraphCommand);

            XFormatter = val => new DateTime((long)val).ToString("dd MMM");
            YFormatter = val => val.ToString("C", CultureInfo.GetCultureInfo("en-us"));
        }

        //-> Commands
        #region Commands
        public ICommand HideCardDetailsCommand { get; }
        public ICommand ShowCardDetailsCommand { get; }
        public ICommand ShowNextCardCommand { get; }
        public ICommand ShowPreviousCardCommand { get; }
        public ICommand ShowNextCardStatementCommand { get; }
        public ICommand ShowPreviousCardStatementCommand { get; }
        public ICommand ToggleZoomModeForGraphCommand { get; }

        private void ExecuteHideCardDetailsCommand(object obj)
        {
            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        private void ExecuteShowCardDetailsCommand(object obj)
        {
            displayCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        private void ExecuteShowNextCardCommand(object obj)
        {

            if (currentCardBeingViewed + 1 < CurrentUserAccount.CreditCards.Count)
            {
                ++currentCardBeingViewed;
                displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
            }
            if (currentCardBeingViewed == CurrentUserAccount.CreditCards.Count - 1)
            {
                IsCardNextButtonVisible = false;
            }
            if (currentCardBeingViewed > 0)
            {
                IsCardPreviousButtonVisible = true;
            }
            fillPreRequisiteData();

            generateDataForGraphDaywise(statementDates[currentStatementView].Item1, statementDates[currentStatementView].Item2, CreditCard.CardID);
        }

        private void ExecuteShowPreviousCardCommand(object obj)
        {

            if (currentCardBeingViewed - 1 >= 0)
            {
                --currentCardBeingViewed;
                displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
            }
            if (currentCardBeingViewed == 0)
            {
                IsCardPreviousButtonVisible = false;
            }
            if (CurrentUserAccount.CreditCards.Count > 1 && currentCardBeingViewed != CurrentUserAccount.CreditCards.Count - 1)
            {
                IsCardNextButtonVisible = true;
            }
            fillPreRequisiteData();

            generateDataForGraphDaywise(statementDates[currentStatementView].Item1, statementDates[currentStatementView].Item2, CreditCard.CardID);
        }

        private void ExecuteShowNextCardStatementCommand(object obj)
        {
            if(currentStatementView + 1 < statementDates.Count())
            {
                ++currentStatementView;
            }
            
            if(currentStatementView == statementDates.Count() - 1)
            {
                IsNextStatementButtonVisible = false;
            }

            if(currentStatementView > 0)
            {
                IsPreviousStatementButtonVisible = true;
            }
            StatementTextToBeDisplayed = statementDates[currentStatementView].Item1.ToString("dd-MMM") + " To " + statementDates[currentStatementView].Item2.ToString("dd-MMM") + " " + statementDates[currentStatementView].Item2.ToString("yyyy");
            generateDataForGraphDaywise(statementDates[currentStatementView].Item1, statementDates[currentStatementView].Item2, CreditCard.CardID);
            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        private void ExecuteShowPreviousCardStatementCommand(object obj)
        {
            if(currentStatementView - 1 >= 0)
            {
                --currentStatementView;
            }

            if(currentStatementView == 0)
            {
                IsPreviousStatementButtonVisible = false;
            }

            if(statementDates.Count() > 1 && currentStatementView != statementDates.Count() - 1)
            {
                IsNextStatementButtonVisible = true;
            }
            StatementTextToBeDisplayed = statementDates[currentStatementView].Item1.ToString("dd-MMM") + " To " + statementDates[currentStatementView].Item2.ToString("dd-MMM") + " " + statementDates[currentStatementView].Item2.ToString("yyyy");
            generateDataForGraphDaywise(statementDates[currentStatementView].Item1, statementDates[currentStatementView].Item2, CreditCard.CardID);
            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        private void ExecuteToggleZoomModeForGraphCommand(object obj)
        {
            ++timesClickedOnZoomToggle;
            timesClickedOnZoomToggle %= 4;

            if (timesClickedOnZoomToggle == 0) ZoomingMode = ZoomingOptions.None;
            else if (timesClickedOnZoomToggle == 1) ZoomingMode = ZoomingOptions.X;
            else if (timesClickedOnZoomToggle == 2) ZoomingMode = ZoomingOptions.Y;
            else if (timesClickedOnZoomToggle == 3) ZoomingMode = ZoomingOptions.Xy;
        }

        #endregion

        //-> Functions User Defined

        #region User Defined Functions
        private void displayCard(Guid id)
        {
            CreditCard = cardRepository.ReturnCardDetails(id);
            CardDisplayAmount = "$" + (CreditCard.CreditLimit - TotalAmounntSpentOnCard).ToString();
            CardDisplayText = "Available Balance";

            IsShowButtonVisible = false;
            IsHideButtonVisible = true;
        }

        private void displayMaskedCard(Guid id)
        {
            CreditCard = cardRepository.ReturnMaskedCardDetails(id);
            CardDisplayAmount = "$" + CreditCard.CreditLimit.ToString();
            CardDisplayText = "Credit Limit";

            IsShowButtonVisible = true;
            IsHideButtonVisible = false;
        }

        private void generateDataForGraphDaywise(DateTime date1, DateTime date2, Guid id)
        {
            #region Cartesian Chart Values

            SeriesValues = new ChartValues<DateTimePoint>();

            TotalAmounntSpentOnCard = 0; // To be passed to the doughnut chart.

            var amountsByDateList = new List<KeyValuePair<DateTime, decimal>>();
            amountsByDateList = transactionRepository.ReturnCardTransactionAmountsGroupByDate(date1, date2, id);

            var pos = 0;

            for (var day = date1.Date; day <= date2.Date; day = day.AddDays(1))
            {
                if (pos < amountsByDateList.Count && day == amountsByDateList[pos].Key)
                {
                    SeriesValues.Add(new DateTimePoint(day, Convert.ToDouble(amountsByDateList[pos].Value)));
                    TotalAmounntSpentOnCard += Convert.ToDouble(amountsByDateList[pos].Value);
                    ++pos;
                }
                else
                {
                    SeriesValues.Add(new DateTimePoint(day, 0));
                }
            }

            #endregion

            #region PieChart Values

            var amountsByCategory = new List<KeyValuePair<Guid, decimal>>();
            amountsByCategory = transactionRepository.ReturnCardTransactionAmountsGroupByCategory(date1, date2, id);

            var categoryNames = new List<string>();
            var categoryValues = new List<double>();


            foreach (var item in amountsByCategory)
            {
                var catName = categoryRepository.GetCategoryName(item.Key);
                categoryNames.Add(catName);

                categoryValues.Add(Convert.ToDouble(item.Value));
            }

            DoughnutChartValues = categoryValues;
            DoughnutChartCategoryNames = categoryNames;

            #endregion
        }

        private void fillPreRequisiteData()
        {
            //Billing Cycle is taken as 30 days
            var statementDay = CreditCard.StatementGenDate;

            earliestTransactionDate = transactionRepository.ReturnEarliestTransactionDateOnCard(CreditCard.CardID);
            latestTransactionDate = transactionRepository.ReturnLatestTransactionDateOnCard(CreditCard.CardID);


            if(earliestTransactionDate.Day < statementDay)
            {
                firstStatementDate2 = new DateTime(earliestTransactionDate.Year, earliestTransactionDate.Month, statementDay).AddDays(-1);
                firstStatementDate1 = firstStatementDate2.AddDays(-30);
            }
            else
            {
                firstStatementDate1 = new DateTime(earliestTransactionDate.Year, earliestTransactionDate.Month, statementDay);
                firstStatementDate2 = firstStatementDate1.AddDays(30);
            }
            statementDates = new List<Tuple<DateTime, DateTime>>();

            statementDates.Add(Tuple.Create(firstStatementDate1, firstStatementDate2));
            statementDate1 = firstStatementDate1;
            statementDate2 = firstStatementDate2;
            while(statementDate2 < latestTransactionDate)
            {
                statementDate1 = statementDate2.AddDays(1);
                statementDate2 = statementDate1.AddDays(30);

                statementDates.Add(Tuple.Create(statementDate1,statementDate2));
            }
            currentStatementView = statementDates.Count() - 1;
            IsNextStatementButtonVisible = false;
            if(currentStatementView > 0)
            {
                IsPreviousStatementButtonVisible = true;
            }

            StatementTextToBeDisplayed = statementDates[currentStatementView].Item1.ToString("dd-MMM") + " To " + statementDates[currentStatementView].Item2.ToString("dd-MMM") + " " + statementDates[currentStatementView].Item2.ToString("yyyy");

        }

        #endregion
    }
}
