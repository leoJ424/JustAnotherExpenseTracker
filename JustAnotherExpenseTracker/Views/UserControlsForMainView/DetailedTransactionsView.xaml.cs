using JustAnotherExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JustAnotherExpenseTracker.Views.UserControlsForMainView
{
    /// <summary>
    /// Interaction logic for DetailedTransactionsView.xaml
    /// </summary>
    public partial class DetailedTransactionsView : UserControl
    {
        public DetailedTransactionsView()
        {
            InitializeComponent();
        }
        private void btnOpenStartDateCalendar_Click(object sender, RoutedEventArgs e)
        {
            startDateCalendarPopup.IsOpen = true;

            var viewModel = (DetailedTransactionsViewModel)DataContext;
            viewModel.StartDateDisplay = viewModel.StartDate.ToString("dd-MMM-yyyy");            
        }

        private void startDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            startDateCalendarPopup.IsOpen = false;

            var viewModel = (DetailedTransactionsViewModel)DataContext;
            if (viewModel != null)
            {
                viewModel.StartDateDisplay = viewModel.StartDate.ToString("dd-MMM-yyyy");
            }
        }

        private void btnOpenEndDateCalendar_Click(object sender, RoutedEventArgs e)
        {
            endDateCalendarPopup.IsOpen = true;

            var viewModel = (DetailedTransactionsViewModel)DataContext;
            viewModel.EndDateDisplay = viewModel.EndDate.ToString("dd-MMM-yyyy");
        }

        private void endDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            endDateCalendarPopup.IsOpen = false;

            var viewModel = (DetailedTransactionsViewModel)DataContext;
            if (viewModel != null)
            { 
                viewModel.EndDateDisplay = viewModel.EndDate.ToString("dd-MMM-yyyy");
            }

        }

        private void btnCloseStartCalendar_Click(object sender, RoutedEventArgs e)
        {
            startDateCalendarPopup.IsOpen = false;
        }

        private void btnCloseEndDateCalendar_Click(object sender, RoutedEventArgs e)
        {
            endDateCalendarPopup.IsOpen = false;
        }

        
        private ObservableCollection<string> _filteredItems;
        private ObservableCollection<string> _items;
        private void btnShowAvailableCards_Click(object sender, RoutedEventArgs e)
        {
            availableCardsPopup.IsOpen = true;
        }

        private void btnCloseCardsList_Click(object sender, RoutedEventArgs e)
        {
            availableCardsPopup.IsOpen = false;
        }

        private void cardNamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           availableCardsPopup.IsOpen = false;

            var viewModel = (DetailedTransactionsViewModel)DataContext;
            if (viewModel != null && _items != null)
            {
                if (_items.IndexOf((string)cardNamesList.SelectedItem) >= 0)
                {
                    viewModel.CurrentCard = viewModel.Cards[_items.IndexOf((string)cardNamesList.SelectedItem)];
                }
                cardNamesList.ItemsSource = _items; //By default beacuse of the "search effect" the entire list of cards will not be available. Once the Selection in the list is changes the list box is reset to original set of items.
            }
        }

        private void txtBoxCardName_TextChanged(object sender, TextChangedEventArgs e)
        {
           if (_items == null) _items = (ObservableCollection<string>)cardNamesList.ItemsSource;
            string filterText = txtBoxCardName.Text;
            if (_items == null) return;
            if (!string.IsNullOrEmpty(filterText))
            {
                var tempItems = _items.Where(item => item.ToLower().Contains(filterText.ToLower())).ToList();
                _filteredItems = new ObservableCollection<string>();

                foreach (var item in tempItems)
                {
                    _filteredItems.Add(item);
                }
                cardNamesList.ItemsSource = _filteredItems;
            }
            else
            {
                cardNamesList.ItemsSource = _items;
            }

            availableCardsPopup.IsOpen = true;

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (DetailedTransactionsViewModel)DataContext;
            await viewModel.Initialize();

        }
    }
}
