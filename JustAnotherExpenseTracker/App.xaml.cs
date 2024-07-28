using JustAnotherExpenseTracker.Services;
using JustAnotherExpenseTracker.ViewModels;
using JustAnotherExpenseTracker.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace JustAnotherExpenseTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainView>(provider => new MainView
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            services.AddTransient<MainViewModel>();
            services.AddTransient<CardsViewModel>();
            services.AddTransient<CardsNotAvailableViewModel>();
            services.AddTransient<DetailedTransactionsViewModel>();
            services.AddTransient<StocksViewModel>();
            services.AddTransient<BanksViewModel>();
            services.AddTransient<DashboardViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }
        protected void ApplicationStart(object sender, StartupEventArgs e)
        {
            //var loginView = new LoginView();
            //loginView.Show();
            //loginView.IsVisibleChanged += (s, ev) =>
            //{
            //    if (loginView.IsVisible == false && loginView.IsLoaded)
            //    {
            //        var mainView = new MainView();
            //        mainView.Show();
            //        loginView.Close();
            //    }
            //};
            //var mainView = new MainView();
            //mainView.Show();

            var mainView = _serviceProvider.GetService<MainView>();
            mainView.Show();
        }
    }

}
