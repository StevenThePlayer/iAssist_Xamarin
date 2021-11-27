using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.ViewModels;
using iAssist_Xamarin.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace iAssist_Xamarin
{
    public partial class AppShellWorker : Shell
    {
        public bool isWorker;
        public AppShellWorker()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(ComplaintPage), typeof(ComplaintPage));
            Routing.RegisterRoute(nameof(CreateTaskPage), typeof(CreateTaskPage));
            Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
            Routing.RegisterRoute(nameof(FindWorkerPage), typeof(FindWorkerPage));
            Routing.RegisterRoute(nameof(MyTaskPage), typeof(MyTaskPage));
            Routing.RegisterRoute(nameof(RateWorkerPage), typeof(RateWorkerPage));
            //Routing.RegisterRoute(nameof(SearchWorkerPage), typeof(SearchWorkerPage));
            Routing.RegisterRoute(nameof(ViewBiddingPage), typeof(ViewBiddingPage));
            Routing.RegisterRoute(nameof(WorkerDetailsPage), typeof(WorkerDetailsPage));

            Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
            Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));

            Routing.RegisterRoute(nameof(CreateBidPage), typeof(CreateBidPage));
            Routing.RegisterRoute(nameof(EditBidPage), typeof(EditBidPage));
            Routing.RegisterRoute(nameof(ViewBiddingWorkerPage), typeof(ViewBiddingWorkerPage));
            Routing.RegisterRoute(nameof(ViewBidRequestPage), typeof(ViewBidRequestPage));

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

            isWorker = false;
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Settings.AccessToken = null;
            Settings.Email = null;
            Settings.Password = null;
            await Current.GoToAsync("//LoginPage");
        }
    }
}
