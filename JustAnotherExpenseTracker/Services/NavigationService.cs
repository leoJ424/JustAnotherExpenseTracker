using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Services
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        //Inheriting the ViewModelBase for the OnPropertyChanged 

        private ViewModelBase _currentView;
        private PassDataModel_DetailedTransactionsView _passedDataForDetailedTransactionsView;
        private readonly Func<Type, ViewModelBase> _viewModelNavigator;

        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public PassDataModel_DetailedTransactionsView PassedDataForDetailedTransactionsView
        {
            get { return _passedDataForDetailedTransactionsView; }
            set
            {
                _passedDataForDetailedTransactionsView = value;
                OnPropertyChanged(nameof(PassedDataForDetailedTransactionsView));
            }
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase viewModel = _viewModelNavigator.Invoke(typeof(TViewModel));

            CurrentView = viewModel;
        }

        public void NavigateTo<TViewModel>(PassDataModel_DetailedTransactionsView obj) where TViewModel : ViewModelBase
        {
            PassedDataForDetailedTransactionsView = obj;

            ViewModelBase viewModel = _viewModelNavigator.Invoke(typeof(TViewModel));

            CurrentView = viewModel;           
        }

        public NavigationService(Func<Type, ViewModelBase> viewModelNavigator)
        {
            _viewModelNavigator = viewModelNavigator;
        }
    }
}
