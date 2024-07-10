using JustAnotherExpenseTracker.ViewModels;
using System;
using System.Collections.Generic;
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
            //if(startDateCalendarPopup.IsOpen == false)
            //{
            //    startDateCalendarPopup.IsOpen = true;
            //}
            //else
            //{
            //    startDateCalendarPopup.IsOpen = false;
            //}
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
            if(endDateCalendarPopup.IsOpen == false)
            {
                endDateCalendarPopup.IsOpen = true;
            }                
            else
            {
                endDateCalendarPopup.IsOpen = false;
            }

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

    }
}
