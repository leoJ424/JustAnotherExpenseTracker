using System;
using System.Collections.Generic;
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
    /// Interaction logic for CardsView.xaml
    /// </summary>
    public partial class CardsView : UserControl
    {
        public CardsView()
        {
            InitializeComponent();
        }

        private void btnResetZoomMode_Click(object sender, RoutedEventArgs e)
        {
            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = 0;
            Y.MaxValue = double.NaN;
        }
    }
}
