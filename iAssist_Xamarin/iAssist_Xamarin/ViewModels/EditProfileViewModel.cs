using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    class EditProfileViewModel : ViewModelBase
    {
        public Command EditCommand { get; }
        private string email, firstname, lastname, contactnumber, userid, message;

        public EditProfileViewModel()
        {
            Title = "Edit Profile Details";
            Message = "";
            IsBusy = true;
            LoadData();
            IsBusy = false;
            EditCommand = new Command(OnEditClicked);
            GetBalance();
        }

        public async void LoadData()
        {
            AccountServices accountServices = new AccountServices();
            EditProfileModel data = await accountServices.GetEditProfileAsync();

            if (string.IsNullOrEmpty(data.Email))
            {
                Message = "An error has occured";
            }

            Email = data.Email;
            Firstname = data.Firstname;
            Lastname = data.Lastname;
            Contactnumber = data.Phonenumber;
            userid = data.Userid;
            //profilePicture = data.ProfilePicture;
        }

        private async void OnEditClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (String.IsNullOrWhiteSpace(Firstname))
            {
                Message = "Enter First Name.";
            }
            else if (String.IsNullOrWhiteSpace(Lastname))
            {
                Message = "Enter Last Name.";
            }
            else if (String.IsNullOrWhiteSpace(Contactnumber))
            {
                Message = "Enter Contact Number.";
            }
            else
            {
                IsBusy = true;
                AccountServices accountServices = new AccountServices();
                bool success = await accountServices.PostEditProfileAsync(Email, Firstname, Lastname, Contactnumber, userid);
                if (success == false)
                {
                    Message = accountServices.Message;
                }
                else
                {
                    Message = "Profile Change Successful";
                }
                IsBusy = false;
            }
        }

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
    }
}
