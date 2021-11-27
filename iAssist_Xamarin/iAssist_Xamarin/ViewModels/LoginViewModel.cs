using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command PageAppearingCommand { get; }
        private string email, password, message;
        AccountServices accountServices = new AccountServices();
        public LoginViewModel()
        {
            Title = "iAssist";
            Message = "";
            IsBusy = false;
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            PageAppearingCommand = new Command(OnAppearing);
        }

        private async void OnAppearing(object obj)
        {
            Message = "";
            await Login();
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if(string.IsNullOrWhiteSpace(Email))
            {
                Message = "Enter an email address.";
            }
            else if(string.IsNullOrWhiteSpace(Password))
            {
                Message = "Enter a password.";
            }
            else
            {
                Settings.Email = Email;
                Settings.Password = Password;
                Login();
            }
        }

        private async Task<bool> Login()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(Settings.Email) || string.IsNullOrWhiteSpace(Settings.Password))
                return false;

            bool result = await accountServices.LoginAsync(Settings.Email, Settings.Password);
            Message = accountServices.Message;

            if (string.IsNullOrEmpty(Settings.AccessToken) == false && result)
            {
                await accountServices.GetRole(Email);
                if(Settings.Role == "Worker")
                    Application.Current.MainPage = new AppShellWorker();

                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                return true;
            }
            IsBusy = false;
            return false;
        }

        private async void OnRegisterClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        public string Message
        {
            get => message;
            set =>SetProperty(ref message, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
    }
}
