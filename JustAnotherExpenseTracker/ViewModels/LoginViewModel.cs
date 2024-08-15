using JustAnotherExpenseTracker.Models;
using JustAnotherExpenseTracker.Models.API_Models;
using JustAnotherExpenseTracker.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JustAnotherExpenseTracker.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string _userName;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;


        //Property
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public SecureString Password
        {
            get
            {
                return _password;
            }
            set 
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get 
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsViewVisible
        {
            get 
            {
                return _isViewVisible;
            }
            set 
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }


        //Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelAsyncCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            ApiHelper.InitializeClient();
        }
        private async Task ExecuteLoginCommand(object obj)
        {
            string url = "api/Account/login";
            using StringContent jsonContent = new (JsonSerializer.Serialize(new
                                                    {
                                                        username = UserName,
                                                        password = new NetworkCredential(UserName, Password).Password,
                                                    }),
                                                    Encoding.UTF8,
                                                    "application/json");

            using (HttpResponseMessage response = await ApiHelper.ApiClient.PostAsync(url, jsonContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponseToken = await response.Content.ReadAsAsync<JwtToken>();
                    ApiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + jsonResponseToken.Token);
                    IsViewVisible = false;
                }
                else
                {
                    ErrorMessage = "* Invalid Username or Password";
                    //throw new Exception(response.ReasonPhrase);
                }

                //var token = ApiHelper.ApiClient.DefaultRequestHeaders.Authorization.Parameter;
                //var handler = new JwtSecurityTokenHandler();
                //var jwtToken = handler.ReadJwtToken(token);
                //var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                //string newUrl = "api/User";
                //using (HttpResponseMessage response2 = await ApiHelper.ApiClient.GetAsync(url))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var res = await response2.Content.ReadAsAsync<UserModel_API>();
                //    }
                //    else
                //    {
                //        throw new Exception(response2.ReasonPhrase);
                //    }
                //}

            }
            //END
            //var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(UserName, Password));
            //if (isValidUser)
            //{
            //    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), null);
            //    IsViewVisible = false;
            //}
            //else
            //{
            //    ErrorMessage = "* Invalid Username or Password";
            //}
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool vaildData;
            if(string.IsNullOrWhiteSpace(UserName) || UserName.Length < 3 || Password == null || Password.Length < 3) 
            {
                vaildData = false;
            }
            else
            {
                vaildData = true;
            }

            return vaildData;
        }

        
    }
}
