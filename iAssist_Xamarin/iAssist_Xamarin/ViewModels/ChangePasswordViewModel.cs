using iAssist_Xamarin.Services;
using System;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        public Command ChangePasswordCommand { get; }
        private string oldPassword, newPassword, confirmNewPassword, message;

        private AccountServices accountServices = new AccountServices();
        public ChangePasswordViewModel()
        {
            Title = "Change Password";
            Message = "";
            IsBusy = false;
            ChangePasswordCommand = new Command(OnEditClicked);
        }


        private async void OnEditClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (String.IsNullOrWhiteSpace(OldPassword))
            {
                Message = "Enter Old Password.";
            }
            else if (String.IsNullOrWhiteSpace(NewPassword))
            {
                Message = "Enter New Password.";
            }
            else if (String.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                Message = "Confirm New Password is required.";
            }
            else if (String.Equals(NewPassword, ConfirmNewPassword) == false)
            {
                Message = "Password and Confirm Password is not the same.";
            }
            else if (NewPassword.Length < 6)
            {
                Message = "The new password must be at least 6 characters long.";
            }
            else
            {
                IsBusy = true;
                bool success = await accountServices.PostChangePassword(oldPassword, newPassword, confirmNewPassword);
                if (success == false)
                {
                    Message = accountServices.Message;
                }
                else
                {
                    Message = "Password Change Successful";
                }
                IsBusy = false;
            }
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public string OldPassword
        {
            get => oldPassword;
            set => SetProperty(ref oldPassword, value);
        }

        public string NewPassword
        {
            get => newPassword;
            set => SetProperty(ref newPassword, value);
        }
        public string ConfirmNewPassword
        {
            get => confirmNewPassword;
            set => SetProperty(ref confirmNewPassword, value);
        }
    }
}
