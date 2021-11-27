using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
namespace iAssist_Xamarin.ViewModels
{
    public class RegisterViewModel : IAddressAutoComplete
    {
        public Command RegisterCommand { get; }
        public Command AddressChangeCommand { get; }
        public Command ListViewTapped { get; }
        private string email, password, confirmPassword, firstname, lastname, contactnumber, address, message;
        public bool ListIsVisible { get; set; }

        public ObservableCollection<string> source = new ObservableCollection<string>();

        public RegisterViewModel()
        {
            Title = "Create an Account";
            Message = "";
            IsBusy = false;

            RegisterCommand = new Command(OnRegisterClicked);
        }

        public async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (string.IsNullOrWhiteSpace(Email))
            {
                Message = "Enter an email address.";
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                Message = "Enter a password.";
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Message = "Confirm Password is required.";
            }
            else if (string.IsNullOrWhiteSpace(Firstname))
            {
                Message = "Enter First Name.";
            }
            else if (string.IsNullOrWhiteSpace(Lastname))
            {
                Message = "Enter Last Name.";
            }
            else if (string.IsNullOrWhiteSpace(Contactnumber))
            {
                Message = "Enter Contact Number.";
            }
            else if (string.IsNullOrWhiteSpace(TempAddress.Address))
            {
                Message = "Enter an Address.";
            }
            else if (string.Equals(ConfirmPassword, Password) == false)
            {
                Message = "Password and Confirm Password is not the same.";
            }
            else
            {
                IsBusy = true;
                bool IsAddressValid = await GetLocation();
                if (IsAddressValid == false)
                {
                    IsBusy = false;
                    Message = "Enter a valid Address.";
                    return;
                }
                AccountServices accountServices = new AccountServices();
                bool results = await accountServices.RegisterUserAsync(Email, Password, ConfirmPassword, Firstname, TempAddress.Address, latitude, longitude, Lastname, Contactnumber);

                Message = accountServices.Message;
                IsBusy = false;
                if (results)
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    await Shell.Current.DisplayAlert("Action Result", Message, "Ok");
                }
            }
        }
        /*
        public async void GetLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    var cts = new CancellationTokenSource();
                    location = await Geolocation.GetLocationAsync(request, cts.Token);

                    if (location != null)
                    {
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }*/

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
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

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }
        public string Firstname
        {
            get => firstname;
            set => SetProperty(ref firstname, value);
        }
        public string Lastname
        {
            get => lastname;
            set => SetProperty(ref lastname, value);
        }
        public string Contactnumber
        {
            get => contactnumber;
            set => SetProperty(ref contactnumber, value);
        }
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }
    }
}