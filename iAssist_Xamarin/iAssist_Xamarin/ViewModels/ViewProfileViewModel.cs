using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
namespace iAssist_Xamarin.ViewModels
{
    class ViewProfileViewModel : ViewModelBase
    {
        UploadFileServices uploadFileServices;
        public Command EditCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command PageAppearingCommand { get; }
        public Command UploadFileCommand { get; }
        private string email, firstname, lastname, contactnumber, address, profilePicture, message;
        private DateTime createdAt, updatedAt;

        public ViewProfileViewModel()
        {
            Message = "";
            Load();

            uploadFileServices = new UploadFileServices();

            EditCommand = new Command(OnEditClicked);
            ChangePasswordCommand = new Command(OnChangePasswordClicked);
            PageAppearingCommand = new Command(OnAppearing);
            UploadFileCommand = new Command(OnUploadFileClicked);
        }
        
        public void OnAppearing()
        {
            Message = "";
            Load();
            GetBalance();
        }

        public async void Load()
        {
            IsBusy = true;

            AccountServices accountServices = new AccountServices();
            ViewProfileModel data = await accountServices.GetProfileAsync();
            if (data == null)
                Message = "An error has occured";
            Email = data.Email;
            Firstname = data.Firstname;
            Lastname = data.Lastname;
            Contactnumber = data.Phonenumber;
            Address = data.Address;

            if(string.IsNullOrWhiteSpace(data.ProfilePicture))
            {
                ProfilePicture = Constants.BaseApiAddress + "image/defaultprofilepic.jpg";
            }
            else
            {
                ProfilePicture = uploadFileServices.ConvertImageUrl(data.ProfilePicture);
            }
            
            CreatedAt = data.Created_At;
            UpdatedAt = data.Updated_At;
            Title = Firstname + "'s Profile";

            IsBusy = false;
        }

        private async void OnUploadFileClicked(object sender)
        {
            string url;
            await uploadFileServices.SelectFile();
            string address = Constants.BaseApiAddress + "api/Manage/UploadProfilePicture";
            _ = await uploadFileServices.SelectFile();
            url = await uploadFileServices.UploadFile(address);
            if(string.IsNullOrWhiteSpace(url) == false)
            {
                ProfilePicture = url;
            }
        }

        private async void OnEditClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(EditProfilePage)}");
        }

        private async void OnChangePasswordClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(ChangePasswordPage)}");
        }

        public string ProfilePicture
        {
            get => profilePicture;
            set => SetProperty(ref profilePicture, value);
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
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }
        public DateTime CreatedAt
        {
            get => createdAt;
            set => SetProperty(ref createdAt, value);
        }
        public DateTime UpdatedAt
        {
            get => updatedAt;
            set => SetProperty(ref updatedAt, value);
        }
    }
}
