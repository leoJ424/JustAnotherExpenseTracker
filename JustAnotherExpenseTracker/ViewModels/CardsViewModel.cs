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
using JustAnotherExpenseTracker.Services;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class CardsViewModel:ViewModelBase
    {
        private INavigationService _navigation;

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
        private string _cardName;
        
        private bool _isMonthlyButtonChecked = true;//By Default true
        private bool _isYearlyButtonChecked = false;//By Default False
        private bool _isNextStatementButtonVisible = true; //By Default false
        private bool _isPreviousStatementButtonVisible = true; //By Default false

        private string _statementTextToBeDisplayed;

        private ChartValues<DateTimePoint> _daywiseDebitSeriesValues;
        private ChartValues<DateTimePoint> _daywiseCreditSeriesValues;
        private ChartValues<DateTimePoint> _monthwiseDebitSeriesValues;
        private ChartValues<DateTimePoint> _monthwiseCreditSeriesValues;

        private ZoomingOptions _zoomingMode;

        private List<double> _doughnutChartValues;
        private List<string> _doughnutChartCategoryNames;
        private double _totalAmountSpentOnCard;

        private bool _chartIsDisplayed = true; //By Default true...if no data then it will be set to false
        private bool _noDataDisplay = false; // Basically the negation of _chartIsDisplayed
        private bool _viewCreditLineSeries = false;
        private bool _viewCreditMonthylyLineSeries = false;

        private int currentCardBeingViewed = 0; // by default user views his/her first card itself

        List<Tuple<DateTime, DateTime>> statementDates;

        private int currentStatementView;

        private int timesClickedOnZoomToggle;

        public Func<double, string> XFormatterMonthwise { get; set; }
        public Func<double, string> XFormatterYearwise { get; set; }
        public Func<double, string> YFormatter { get; set; }


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

        public ChartValues<DateTimePoint> DaywiseDebitSeriesValues
        {
            get
            {
                return _daywiseDebitSeriesValues;
            }
            set
            {
                _daywiseDebitSeriesValues = value;
                OnPropertyChanged(nameof(DaywiseDebitSeriesValues));
            }
        }

        public ChartValues<DateTimePoint> DaywiseCreditSeriesValues
        {
            get
            {
                return _daywiseCreditSeriesValues;
            }
            set
            {
                _daywiseCreditSeriesValues = value;
                OnPropertyChanged(nameof(DaywiseCreditSeriesValues));
            }
        }

        public ChartValues<DateTimePoint> MonthwiseDebitSeriesValues
        {
            get
            {
                return _monthwiseDebitSeriesValues;
            }
            set
            {
                _monthwiseDebitSeriesValues = value;
                OnPropertyChanged(nameof(MonthwiseDebitSeriesValues));
            }
        }

        public ChartValues<DateTimePoint> MonthwiseCreditSeriesValues
        {
            get
            {
                return _monthwiseCreditSeriesValues;
            }
            set
            {
                _monthwiseCreditSeriesValues = value;
                OnPropertyChanged(nameof(MonthwiseCreditSeriesValues));
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

        public bool ChartIsDisplayed
        {
            get
            {
                return _chartIsDisplayed;
            }
            set
            {
                _chartIsDisplayed = value;
                OnPropertyChanged(nameof(ChartIsDisplayed));
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

        public bool ViewCreditLineSeries 
        {
            get
            {
                return _viewCreditLineSeries;
            }
            set
            {
                _viewCreditLineSeries = value;
                OnPropertyChanged(nameof(ViewCreditLineSeries));
            }
        }

        public bool ViewCreditMonthlyLineSeries
        {
            get
            {
                return _viewCreditMonthylyLineSeries;
            }
            set
            {
                _viewCreditMonthylyLineSeries = value;
                OnPropertyChanged(nameof(ViewCreditMonthlyLineSeries));
            }
        }



        #endregion

        public CardsViewModel(INavigationService navService)
        {
            Navigation = navService;

            #region Repository Setup

            userRepository = new UserRepository();
            //TO BE DELETED - Implemented to just make it work without logging in each time

            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential("admin", "admin"));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("admin"), null);
            }

            //END


            cardRepository = new CardRepository();
            transactionRepository = new TransactionRepository();
            categoryRepository = new CategoryRepository();

            #endregion

            #region Commands Setup

            HideCardDetailsCommand = new ViewModelCommand(ExecuteHideCardDetailsCommand);
            ShowCardDetailsCommand = new ViewModelCommand(ExecuteShowCardDetailsCommand);

            ShowNextCardCommand = new ViewModelCommand(ExecuteShowNextCardCommand);
            ShowPreviousCardCommand = new ViewModelCommand(ExecuteShowPreviousCardCommand);

            ShowNextCardStatementCommand = new ViewModelCommand(ExecuteShowNextCardStatementCommand); ;
            ShowPreviousCardStatementCommand = new ViewModelCommand(ExecuteShowPreviousCardStatementCommand);

            ToggleZoomModeForGraphCommand = new ViewModelCommand(ExecuteToggleZoomModeForGraphCommand);

            GoToDetailedTransactionDataCommand = new ViewModelCommand(ExecuteGoToDetailedTransactionDataCommand);

            MonthlyButtonClickedCommand = new ViewModelCommand(ExecuteMonthlyButtonClickedCommand);
            YearlyButtonClickedCommand = new ViewModelCommand(ExecuteYearlyButtonClickedCommand);

            #endregion

            #region Formatters for the cartesian graph

            XFormatterMonthwise = val => new DateTime((long)val).ToString("dd MMM");
            XFormatterYearwise = val => new DateTime((long)val).ToString("MMM");
            YFormatter = val => val.ToString("C", CultureInfo.GetCultureInfo("en-us"));

            #endregion

            CurrentUserAccount = new UserAccountModel();
            statementDates = new List<Tuple<DateTime, DateTime>>();

            timesClickedOnZoomToggle = 0; // Based on the number of times this button is clicked we set a particular zoom mode

            CurrentUserAccount = LoadCurrentUserData(Thread.CurrentPrincipal.Identity.Name);
            if(CurrentUserAccount.CreditCards.Count > 1)
            {
                IsCardNextButtonVisible = true;
            }
            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);

            //Since, initially when the page is loaded, by default the "monthly" button is checked hence the cartesian chart must show the daywise data

            fillPreRequisiteDaywiseData();

            generateDataForGraphDaywise();
            
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
        public ICommand GoToDetailedTransactionDataCommand { get; }
        public ICommand MonthlyButtonClickedCommand { get; }
        public ICommand YearlyButtonClickedCommand { get; }

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

            if(IsMonthlyButtonChecked)
            {
                fillPreRequisiteDaywiseData();
                setChartDisplayStatus();
                generateDataForGraphDaywise();
            }
            if(IsYearlyButtonChecked)
            {
                fillPreRequisiteMonthwiseData();
                setChartDisplayStatus();                
                generateDataForGraphMonthwise();       
            }
            
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

            if(IsMonthlyButtonChecked)
            {
                fillPreRequisiteDaywiseData();
                setChartDisplayStatus();
                generateDataForGraphDaywise();                
            }
            if(IsYearlyButtonChecked)
            {
                fillPreRequisiteMonthwiseData();
                setChartDisplayStatus();
                generateDataForGraphMonthwise();
            }
            
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
            if(IsMonthlyButtonChecked)
            {
                setStatementToBeDisplayedProperty();
                generateDataForGraphDaywise();
            }
            if(IsYearlyButtonChecked)
            {
                setStatementToBeDisplayedProperty();
                generateDataForGraphMonthwise();
            }
            
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
            if (IsMonthlyButtonChecked)
            {
                setStatementToBeDisplayedProperty();
                generateDataForGraphDaywise();
            }
            if (IsYearlyButtonChecked)
            {
                setStatementToBeDisplayedProperty();
                generateDataForGraphMonthwise();
            }
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

        private void ExecuteGoToDetailedTransactionDataCommand(object obj)
        {
            var temp = Navigation.CurrentView;
            var passObj = new PassDataModel_DetailedTransactionsView()
            {
                StartDate = statementDates[currentStatementView].Item1,
                EndDate = statementDates[currentStatementView].Item2,
                CurrentCard = CreditCard.CardID,
                CardIDs = CurrentUserAccount.CreditCards
            };
            Navigation.NavigateTo<DetailedTransactionsViewModel>(passObj);

        }

        private void ExecuteMonthlyButtonClickedCommand(object obj)
        {
            fillPreRequisiteDaywiseData();

            generateDataForGraphDaywise();

            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        private void ExecuteYearlyButtonClickedCommand(object obj)
        {
            fillPreRequisiteMonthwiseData();

            generateDataForGraphMonthwise();

            displayMaskedCard(CurrentUserAccount.CreditCards[currentCardBeingViewed]);
        }

        #endregion

        //-> Functions User Defined

        #region User Defined Functions
        private void displayCard(Guid id)
        {
            CreditCard = cardRepository.ReturnCardDetails(id);
            CardName = CreditCard.CardName;
            CardDisplayAmount = "$" + (CreditCard.CreditLimit - TotalAmounntSpentOnCard).ToString();
            CardDisplayText = "Available Balance";

            IsShowButtonVisible = false;
            IsHideButtonVisible = true;
        }

        private void displayMaskedCard(Guid id)
        {
            CreditCard = cardRepository.ReturnMaskedCardDetails(id);
            CardName = CreditCard.CardName;
            CardDisplayAmount = "$" + CreditCard.CreditLimit.ToString();
            CardDisplayText = "Credit Limit";

            IsShowButtonVisible = true;
            IsHideButtonVisible = false;
        }

        private void generateDataForGraphDaywise()
        {
            if (statementDates.Count == 0) //Implies no transaction data at all
            {
                return;
            }

            DateTime date1 = statementDates[currentStatementView].Item1;
            DateTime date2 = statementDates[currentStatementView].Item2;
            Guid id = CreditCard.CardID;

            #region Cartesian Chart Values

            DaywiseDebitSeriesValues = new ChartValues<DateTimePoint>();
            DaywiseCreditSeriesValues = new ChartValues<DateTimePoint>();

            TotalAmounntSpentOnCard = 0; // To be used by the doughnut chart.

            var debitAmountsByDateList = new List<KeyValuePair<DateTime, decimal>>();
            var creditAmountsByDateList = new List<KeyValuePair<DateTime, decimal>>();

            debitAmountsByDateList = transactionRepository.ReturnCardDebitTransactionAmountsGroupByDate(date1, date2, id);
            creditAmountsByDateList = transactionRepository.ReturnCardCreditTransactionAmountsGroupByDate(date1, date2, id);

            if(debitAmountsByDateList.Count() == 0 && creditAmountsByDateList.Count() == 0) // No Transaction Data for the current statement period
            {
                ChartIsDisplayed = false;
            }
            else
            {
                ChartIsDisplayed = true;
            }

            if(creditAmountsByDateList.Count() != 0)
            {
                ViewCreditLineSeries = true;
            }
            else
            { 
                ViewCreditLineSeries = false;
            }

            var posDebit = 0;
            var posCredit = 0;

            for (var day = date1.Date; day <= date2.Date; day = day.AddDays(1))
            {
                if (posDebit < debitAmountsByDateList.Count && day == debitAmountsByDateList[posDebit].Key)
                {
                    DaywiseDebitSeriesValues.Add(new DateTimePoint(day, Convert.ToDouble(debitAmountsByDateList[posDebit].Value)));
                    TotalAmounntSpentOnCard += Convert.ToDouble(debitAmountsByDateList[posDebit].Value);
                    ++posDebit;
                }
                else
                {
                    DaywiseDebitSeriesValues.Add(new DateTimePoint(day, 0));
                }

                if (posCredit < creditAmountsByDateList.Count && day == creditAmountsByDateList[posCredit].Key)
                {
                    DaywiseCreditSeriesValues.Add(new DateTimePoint(day, Convert.ToDouble(creditAmountsByDateList[posCredit].Value)));
                    TotalAmounntSpentOnCard -= Convert.ToDouble(creditAmountsByDateList[posCredit].Value);
                    ++posCredit;
                }
                else
                {
                    DaywiseCreditSeriesValues.Add(new DateTimePoint(day, 0));
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

        /// <summary>
        /// The function generates first generates a list of statement dates based on the the card's :- statement generation date, earliest and latest transaction dates.
        /// It then sets the required properties to show the latest statement available. 
        /// </summary>
        private void fillPreRequisiteDaywiseData()
        {
            //Note : Billing Cycle is taken as 30 days. Can make it a custom thing in future versions. Maybe, maybe not. IDK.

            DateTime statementDate1;
            DateTime statementDate2;

            DateTime firstStatementDate1;
            DateTime firstStatementDate2;

            var statementDay = CreditCard.StatementGenDate;
            statementDates.Clear();

            var earliestTransactionDate = transactionRepository.ReturnEarliestTransactionDateOnCard(CreditCard.CardID);
            var latestTransactionDate = transactionRepository.ReturnLatestTransactionDateOnCard(CreditCard.CardID);

            if(earliestTransactionDate == DateTime.MinValue || latestTransactionDate == DateTime.MinValue)
            {
                //Implies no transaction data so charts have nothing to show
                ChartIsDisplayed = false;
                NoDataDisplay = true;

                return;
            }
            else
            {
                //ChartIsDisplayed = true; //Commented out cause every time a fillPreReqisite function is called a generateDataForGraph function is also called where this property is handled.
                NoDataDisplay = false;
            }

            // If the "day" in the earliest transaction date is before the statement generation day of the card
            // then the very first statement's end date is going to be a day prior to the statement generation day and
            // the start date is going to be 30 days prior to the end date.
            // If that is not the case the first statement is going to start on the statement generation day on the month of when the earliest transaction took place
            // and end 30 dyas later.

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

            

            statementDates.Add(Tuple.Create(firstStatementDate1, firstStatementDate2));
            statementDate1 = firstStatementDate1;
            statementDate2 = firstStatementDate2;
            while(statementDate2 < latestTransactionDate)
            {
                statementDate1 = statementDate2.AddDays(1);
                statementDate2 = statementDate1.AddDays(30);

                statementDates.Add(Tuple.Create(statementDate1,statementDate2));
            }

            //Showing the latest statement initially
            currentStatementView = statementDates.Count() - 1;
            IsNextStatementButtonVisible = false;
            if(currentStatementView > 0)
            {
                IsPreviousStatementButtonVisible = true;
            }
            else
            {
                IsPreviousStatementButtonVisible = false;
            }

            setStatementToBeDisplayedProperty();
        }

        private void fillPreRequisiteMonthwiseData()
        {
            var earliestTransactionDate = transactionRepository.ReturnEarliestTransactionDateOnCard(CreditCard.CardID);
            var latestTransactionDate = transactionRepository.ReturnLatestTransactionDateOnCard(CreditCard.CardID);

            statementDates.Clear(); //Clear any data if existing

            if (earliestTransactionDate == DateTime.MinValue || latestTransactionDate == DateTime.MinValue)
            {
                //Implies no transaction data
                ChartIsDisplayed = false;
                NoDataDisplay = true;

                return;
            }
            else
            {
                //ChartIsDisplayed = true; //Commented out cause every time a fillPreReqisite function is called a generateDataForGraph function is also called where this property is handled.
                NoDataDisplay = false;
            }

            //Creating the list of years to be displayed.
            var earliestYear = earliestTransactionDate.Year;
            var latestYear = latestTransactionDate.Year;

            for (int i = earliestYear; i <= latestYear; ++i)
            {
                statementDates.Add(Tuple.Create(new DateTime(i, 1, 1), new DateTime(i, 12, 31)));
            }

            currentStatementView = statementDates.Count() - 1;
            IsNextStatementButtonVisible = false;
            if (currentStatementView > 0)
            {
                IsPreviousStatementButtonVisible = true;
            }
            else
            {
                IsPreviousStatementButtonVisible = false;
            }

            setStatementToBeDisplayedProperty();
        }

        private void generateDataForGraphMonthwise()
        {
            if(statementDates.Count() == 0) //Implies no transaction data
            {
                return;
            }

            DateTime date1 = statementDates[currentStatementView].Item1;
            DateTime date2 = statementDates[currentStatementView].Item2;
            Guid id = CreditCard.CardID;

            #region For Cartesian Chart

            MonthwiseDebitSeriesValues = new ChartValues<DateTimePoint>();
            MonthwiseCreditSeriesValues = new ChartValues<DateTimePoint>();

            TotalAmounntSpentOnCard = 0; // To be used by the doughnut chart.

            var debitAmountsByMonthList = new List<KeyValuePair<int, decimal>>();
            var creditAmountsByMonthList = new List<KeyValuePair<int, decimal>>();

            debitAmountsByMonthList = transactionRepository.ReturnCardDebitTransactionAmountsGroupByMonth(date1, date2, id);
            creditAmountsByMonthList = transactionRepository.ReturnCardCreditTransactionAmountsGroupByMonth(date1, date2, id);

            if (debitAmountsByMonthList.Count() == 0 && creditAmountsByMonthList.Count() == 0) // No Transaction Data for the current statement period
            {
                ChartIsDisplayed = false;
            }
            else
            {
                ChartIsDisplayed = true;
            }

            if(creditAmountsByMonthList.Count() != 0)
            {
                ViewCreditMonthlyLineSeries = true;
            }
            else
            {
                ViewCreditMonthlyLineSeries = false;
            }

            var posDebit = 0;
            var posCredit = 0;

            for (int month = 1; month <= 12; ++month)
            {
                if (posDebit < debitAmountsByMonthList.Count && month == debitAmountsByMonthList[posDebit].Key)
                {
                    MonthwiseDebitSeriesValues.Add(new DateTimePoint(new DateTime(date1.Year, month, 1), Convert.ToDouble(debitAmountsByMonthList[posDebit].Value))); // Setting the date to the first of every month. When displaying in the chart we can make sure the month is only displayed.
                    TotalAmounntSpentOnCard += Convert.ToDouble(debitAmountsByMonthList[posDebit].Value);
                    ++posDebit;
                }
                else
                {
                    MonthwiseDebitSeriesValues.Add(new DateTimePoint(new DateTime(date1.Year, month, 1), 0));
                }

                if (posCredit < creditAmountsByMonthList.Count && month == creditAmountsByMonthList[posCredit].Key)
                {
                    MonthwiseCreditSeriesValues.Add(new DateTimePoint(new DateTime(date1.Year, month, 1), Convert.ToDouble(creditAmountsByMonthList[posCredit].Value))); // Setting the date to the first of every month. When displaying in the chart we can make sure the month is only displayed.
                    TotalAmounntSpentOnCard -= Convert.ToDouble(creditAmountsByMonthList[posCredit].Value);
                    ++posCredit;
                }
                else
                {
                    MonthwiseCreditSeriesValues.Add(new DateTimePoint(new DateTime(date1.Year, month, 1), 0));
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

        private void setChartDisplayStatus()
        {
            if (statementDates.Count == 0)
            {
                NoDataDisplay = true;
                ChartIsDisplayed = false;
                return;
            }
            else NoDataDisplay = false;
        }

        private void setStatementToBeDisplayedProperty()
        {
            if(IsMonthlyButtonChecked)
            {
                StatementTextToBeDisplayed = statementDates[currentStatementView].Item1.ToString("dd-MMM") + " To " + statementDates[currentStatementView].Item2.ToString("dd-MMM") + " " + statementDates[currentStatementView].Item2.ToString("yyyy");
            }

            if(IsYearlyButtonChecked)
            {
                StatementTextToBeDisplayed = "Year " + statementDates[currentStatementView].Item1.Year;
            }
        }
        #endregion
    }
}
