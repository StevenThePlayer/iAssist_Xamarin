using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using System;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class CreateBidViewModel : ViewModelBase
    {
        public Command CreateBidCommand { get; }

        public int taskId;
        private string bidDescription, message;
        private decimal bidAmount;
        private DateTime selectedDate, currDate;

        public CreateBidViewModel()
        {
            Title = "Create Bid";
            Message = "";
            IsBusy = true;
            IsNotBusy = !IsBusy;

            TaskId = DataKeepServices.GetTaskId();
            CreateBidCommand = new Command(OnCreateClicked);

            CurrDate = DateTime.Now.AddDays(1);
            SelectedDate = CurrDate;

            GetBalance();

            IsBusy = false;
            IsNotBusy = !IsBusy;
        }

        public async void OnCreateClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (string.IsNullOrWhiteSpace(BidDescription))
            {
                Message = "Enter a Task Title.";
            }
            else if(BidDescription.Length < 30)
            {
                Message = "Description must be at least 30 characters.";
            }
            else if (BidAmount == 0)
            {
                Message = "Enter Bid Amount.";
            }
            else if (BidAmount < 100)
            {
                Message = "Bid Amount must be greater than or equal to 100";
            }
            else
            {
                IsBusy = true;
                IsNotBusy = !IsBusy;

                BidServices bidServices = new BidServices();
                bool results = await bidServices.PostCreateBidToTheTask(BidAmount, BidDescription, TaskId, SelectedDate);
                
                IsBusy = true;
                IsNotBusy = !IsBusy;
                if (results)
                {
                    await Shell.Current.GoToAsync($"//{nameof(ViewBidRequestPage)}");
                    await Shell.Current.DisplayAlert("Action Result", $"Task {TaskId} successfully bidded.", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Action Result", $"An error has occured with sender data to the server.", "Ok");
                }
                IsBusy = false;
                IsNotBusy = !IsBusy;
            }
        }

        public int TaskId { get => taskId; set => SetProperty(ref taskId, value); }

        public string Message { get => message; set => SetProperty(ref message, value); }

        public string BidDescription { get => bidDescription; set => SetProperty(ref bidDescription, value); }

        public decimal BidAmount { get => bidAmount; set => SetProperty(ref bidAmount, value); }
        public DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }
        public DateTime CurrDate { get => currDate; set => SetProperty(ref currDate, value); }
    }
}
