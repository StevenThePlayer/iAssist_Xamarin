using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class AppShellViewModel
    {
        public bool isWorker;
        public Command OnAppearing;

        public AppShellViewModel()
        {

            OnAppearing = new Command(OnShellAppearing);
        }

        public void OnShellAppearing()
        {
            GetRole();
        }

        public async void GetRole()
        {
            AccountServices accountServices = new AccountServices();
            await accountServices.GetRole();
            if (Settings.Role == "Worker")
            {
                isWorker = true;
            }
            else
            {
                isWorker = false;
            }
        }
    }
}
