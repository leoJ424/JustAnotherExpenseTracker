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

namespace JustAnotherExpenseTracker.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CreditCard.xaml
    /// </summary>
    public partial class CreditCard : UserControl
    {
        public CreditCard()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty First4DigitsProperty = DependencyProperty.Register("first4Digits", typeof(string), typeof(CreditCard));

        public string first4Digits
        {
            get { return (string)GetValue(First4DigitsProperty); }
            set { SetValue(First4DigitsProperty, value); }
        }

        public static readonly DependencyProperty Second4DigitsProperty = DependencyProperty.Register("second4Digits", typeof(string), typeof(CreditCard));

        public string second4Digits
        {
            get { return (string)GetValue(Second4DigitsProperty); }
            set { SetValue(Second4DigitsProperty, value); }
        }

        public static readonly DependencyProperty Third4DigitsProperty = DependencyProperty.Register("third4Digits", typeof(string), typeof(CreditCard));

        public string third4Digits
        {
            get { return (string)GetValue(Third4DigitsProperty); }
            set { SetValue(Third4DigitsProperty, value); }
        }

        public static readonly DependencyProperty Last4DigitsProperty = DependencyProperty.Register("last4Digits", typeof(string), typeof(CreditCard));

        public string last4Digits
        {
            get { return (string)GetValue(Last4DigitsProperty); }
            set { SetValue(Last4DigitsProperty, value); }
        }

        public static readonly DependencyProperty NetworkImageProperty = DependencyProperty.Register("networkImage", typeof(ImageSource), typeof(CreditCard));

        public ImageSource networkImage
        {
            get { return (ImageSource)GetValue(NetworkImageProperty); }
            set { SetValue(NetworkImageProperty, value); }
        }

        public static readonly DependencyProperty CardholderNameProperty = DependencyProperty.Register("cardholderName", typeof(string), typeof(CreditCard));

        public string cardholderName
        {
            get { return (string)GetValue(CardholderNameProperty); }
            set { SetValue(CardholderNameProperty, value); }
        }

        public static readonly DependencyProperty ExpDateProperty = DependencyProperty.Register("expDate", typeof(string), typeof(CreditCard));

        public string expDate
        {
            get { return (string)GetValue(ExpDateProperty); }
            set { SetValue(ExpDateProperty, value); }
        }

        public static readonly DependencyProperty CVCProperty = DependencyProperty.Register("cvc", typeof(string), typeof(CreditCard));

        public string cvc
        {
            get { return (string)GetValue(CVCProperty); }
            set { SetValue(CVCProperty, value); }
        }

        public static readonly DependencyProperty CreditLimitProperty = DependencyProperty.Register("creditLimit", typeof(string), typeof(CreditCard));

        public string creditLimit
        {
            get { return (string)GetValue(CreditLimitProperty); }
            set { SetValue(CreditLimitProperty, value); }
        }
    }
}
