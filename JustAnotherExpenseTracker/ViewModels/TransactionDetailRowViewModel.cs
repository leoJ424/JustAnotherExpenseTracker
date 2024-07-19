using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class TransactionDetailRowViewModel:ViewModelBase
    {
        private string _categoryName;
        private string _recipientName;
        private string _amount;
        private string _transactionType;
        private double _rewardPoints;
        private string _dateOfTransaction;
        private string _generalComments;

        private bool _showGeneralComments = false;

        #region Properties

        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        public string RecipientName
        {
            get
            {
                return _recipientName;
            }
            set
            {
                _recipientName = value;
                OnPropertyChanged(nameof(RecipientName));
            }
        }
        public string Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public string TransactionType
        {
            get
            {
                return _transactionType;
            }
            set
            {
                _transactionType = value;
                OnPropertyChanged(nameof(TransactionType));
            }
        }

        public double RewardPoints
        {
            get
            {
                return _rewardPoints;
            }
            set
            {
                _rewardPoints = value;
                OnPropertyChanged(nameof(RewardPoints));
            }
        }

        public string DateOfTransaction
        {
            get 
            {
                return _dateOfTransaction;
            }
            set
            {
                _dateOfTransaction = value;
                OnPropertyChanged(nameof(DateOfTransaction));
            }
        }

        public string GeneralComments
        {
            get
            {
                return _generalComments;
            }
            set
            {
                _generalComments = value;
                OnPropertyChanged(nameof(GeneralComments));
            }
        }

        public bool ShowGeneralComments
        {
            get
            {
                return _showGeneralComments;
            }
            set
            {
                _showGeneralComments = value;
                OnPropertyChanged(nameof(ShowGeneralComments));
            }
        }
        #endregion

        #region Commands
        public ICommand ShowGeneralCommentsCommand { get; }
        public ICommand HideGeneralCommentsCommand { get; }
        #endregion

        public TransactionDetailRowViewModel()
        {
            ShowGeneralCommentsCommand = new ViewModelCommand(ExecuteShowGeneralCommentsCommand);
            HideGeneralCommentsCommand = new ViewModelCommand(ExecuteHideGeneralCommentsCommand);
        }

        private void ExecuteShowGeneralCommentsCommand(object obj)
        {
            ShowGeneralComments = true;
            var t1 = CategoryName;
            var t2 = RecipientName;
        }
        private void ExecuteHideGeneralCommentsCommand(object obj)
        {
            ShowGeneralComments = false;
        }

    }
}
