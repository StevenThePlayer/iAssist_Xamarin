using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class EditBidViewModel : CreateBidViewModel
    {
        private BidModel bidData = new BidModel();
        public Command EditBidCommand;
        BidServices bidServices;

        public EditBidViewModel()
        {
            Title = "Edit Bid";
            Message = "";
            IsBusy = true;
            IsNotBusy = !IsBusy;

            bidServices = new BidServices();
            bidData = DataKeepServices.GetBidData();

            EditBidCommand = new Command(OnCreateClicked);

            GetBidData();
            GetBalance();

            IsBusy = false;
            IsNotBusy = !IsBusy;
        }

        public async void GetBidData()
        {
            bidData = await bidServices.GetEditBidding(bidData.Bidid);
            TaskId = bidData.TaskdetId;
            BidDescription = bidData.Bid_Description;
            BidAmount = bidData.Bid_Amount;
        }

        public async void OnEditClicked()
        {

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (string.IsNullOrWhiteSpace(BidDescription))
            {
                Message = "Enter a Task Title.";
            }
            else if (BidAmount == 0)
            {
                Message = "Enter Bid Amount.";
            }
            else
            {
                IsBusy = true;
                IsNotBusy = !IsBusy;

                bool results = await bidServices.PostEditBidding(BidAmount, BidDescription, bidData);

                IsBusy = true;
                IsNotBusy = !IsBusy;
                if (results)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                    await Shell.Current.DisplayAlert("Action Result", $"Bid for task {TaskId} successfully edited.", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Action Result", $"An error has occured with sender data to the server.", "Ok");
                }
                IsBusy = false;
                IsNotBusy = !IsBusy;
            }
        }
    }
}
