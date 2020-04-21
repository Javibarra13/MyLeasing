using MyLeasing.Common.Models;
using MyLeasing.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace MyLeasing.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnable;
        private DelegateCommand _loginCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Login";
            IsEnable = true;
        }
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password { get => _password; set => SetProperty(ref _password, value); }

        public bool IsRunning { get => _isRunning; set => SetProperty(ref _isRunning, value); }

        public bool IsEnable { get => _isEnable; set => SetProperty(ref _isEnable, value); }

        private async void Login()
        {
            if(string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an email", "Acept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter a password", "Acept");
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            IsRunning = true;
            IsEnable = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.GetTokenAsync(url, "Account", "/CreateToken", request);

            if(!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "User or Password incorrect", "Acept");
                Password = string.Empty;
                return;
            }

            IsRunning = false;
            IsEnable = true;

            var token = response.Result;

            await App.Current.MainPage.DisplayAlert("Ok", "Fuck Yeah !!!", "Acept");
        }

    }
}
