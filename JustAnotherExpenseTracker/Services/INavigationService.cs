using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustAnotherExpenseTracker.Services
{
    public interface INavigationService
    {
        ViewModelBase CurrentView { get; }
        PassDataModel_DetailedTransactionsView PassedDataForDetailedTransactionsView { get; }
        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateTo<T>(PassDataModel_DetailedTransactionsView obj) where T : ViewModelBase;
    }
}
