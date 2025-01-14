﻿using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class ViewBiddingViewModel : ViewModelBase
    {
        private BidServices bidServices;
        public List<BidModel> bidList;
        private string selectedCategory;

        public ObservableRangeCollection<BidModel> BidList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> SelectedCommand { get; }

        public Command SortByCommand { get; }
        public Command PageAppearing { get; }
        public Command FilterCommand { get; }

        public AsyncCommand<BidModel> AcceptBidCommand { get; }
        public AsyncCommand<BidModel> CancelBiddingCommand { get; }
        public AsyncCommand<BidModel> WorkerDetailsCommand { get; }
        
        private int categorySelected;
        private decimal minimum, maximum;

        public ViewBiddingViewModel()
        {
            Title = "Task Bids from Workers";

            BidList = new ObservableRangeCollection<BidModel>();
            SortBy = new ObservableCollection<string>();

            bidServices = new BidServices();

            RefreshCommand = new AsyncCommand(Refresh);
            SortByCommand = new Command(Load);
            PageAppearing = new Command(OnAppearing);
            FilterCommand = new Command(OnFilter);

            AcceptBidCommand = new AsyncCommand<BidModel>(OnAcceptBid);
            CancelBiddingCommand = new AsyncCommand<BidModel>(OnCancelBid);
            WorkerDetailsCommand = new AsyncCommand<BidModel>(OnWorkerClicked);

            SortByBidInit();
            GetBids();
            CategoryInit();
        }

        public void OnAppearing()
        {
            GetBids();
            GetBalance();
        }

        public async Task Refresh()
        {
            IsBusy = true;

            GetBids();

            IsBusy = false;
        }

        public virtual async void GetBids()
        {
            bidList = await bidServices.GetViewBidding(DataKeepServices.GetMyTaskData().Id);
            Load();
        }

        public async void OnFilter()
        {
            if (Maximum >= Minimum)
            {
                if (SelectedCategory != "All")
                    bidList = await bidServices.GetViewBidding(DataKeepServices.GetMyTaskData().Id, SelectedCategory, 1, minimum, maximum);
                else
                    bidList = await bidServices.GetViewBidding(DataKeepServices.GetMyTaskData().Id, "", 1, minimum, maximum);
                Load();
            }
        }

        public void Load()
        {
            UploadFileServices fileServices = new UploadFileServices();

            if (BidList != null)
                BidList.Clear();

            if (bidList != null)
            {
                foreach (var data in bidList)
                {
                    if (string.IsNullOrWhiteSpace(data.ProfilePicture))
                    {
                        data.ProfilePicture = "defaultprofilepic.jpg";
                    }
                    else
                    {
                        data.ProfilePicture = fileServices.ConvertImageUrl(data.ProfilePicture);
                    }
                    BidList.Add(new BidModel
                    {
                        Bidid = data.Bidid,
                        Bid_Amount = data.Bid_Amount,
                        Bid_Description = data.Bid_Description,
                        TaskdetId = data.TaskdetId,
                        Firstname = data.Firstname,
                        Lastname = data.Lastname,
                        ProfilePicture = data.ProfilePicture,
                        Tasktitle = data.Tasktitle,
                        user = data.user,
                        workerid = data.workerid,
                        Username = data.Username,
                        bookstatus = data.bookstatus,
                        Rate = data.Rate
                    });
                }
            }
            /*
            if (SelectedCategory == "5 Stars")
            {
                categorySelected = 5;
            }
            else if (SelectedCategory == "4 Stars and Up")
            {
                categorySelected = 4;
            }
            else if (SelectedCategory == "3 Stars and Up")
            {
                categorySelected = 3;
            }
            else if (SelectedCategory == "2 Stars and Up")
            {
                categorySelected = 2;
            }
            else if (SelectedCategory == "All")
            {
                categorySelected = 0;
            }

            if (bidList != null)
            {
                foreach (var data in bidList)
                {
                    data.ProfilePicture = fileServices.ConvertImageUrl(data.ProfilePicture);
                    if((int)data.Rate >= categorySelected)
                    {
                        BidList.Add(new BidModel
                        {
                            Bidid = data.Bidid,
                            Bid_Amount = data.Bid_Amount,
                            Bid_Description = data.Bid_Description,
                            TaskdetId = data.TaskdetId,
                            Firstname = data.Firstname,
                            Lastname = data.Lastname,
                            ProfilePicture = data.ProfilePicture,
                            Tasktitle = data.Tasktitle,
                            user = data.user,
                            workerid = data.workerid,
                            Username = data.Username,
                            bookstatus = data.bookstatus,
                            //Rate = data.Rate
                        });
                    }
                }
            }*/
        }
        public virtual async Task OnAcceptBid(BidModel bid)
        {
            if (bid == null)
                return;

            if (bid.Bid_Amount > Balance)
            {
                await Shell.Current.DisplayAlert("Insufficient Balance", $"Not enough balance, Current balance: {Balance} Php, Required Amount: {bid.Bid_Amount} Php", "Ok");
                return;
            }

            DisplaySelect("Accept Bid", $"Accept Bid: {bid.Bid_Amount} from worker {bid.Lastname}, {bid.Firstname}", "Bid Accepted", "Bid Failed", bidServices.AcceptBid, bid.Bidid, bid.TaskdetId);

            await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
        }

        public virtual async Task OnCancelBid(BidModel bid)
        {
            if (bid == null)
                return;

            DisplaySelect("Cancel Bid?", $"Cancel Bid for Task {bid.Tasktitle}", "Task Bid Canceled", "Cancel Failed", bidServices.CancelBid, bid.Bidid, bid.TaskdetId);

            await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
        }

        public virtual async Task OnWorkerClicked(BidModel bid)
        {
            if (bid == null)
                return;

            var data = new SearchWorkerModel
            {
                WorkerId = (int)bid.workerid,
                Taskdet = bid.TaskdetId
            };

            DataKeepServices.SetWorkerData(data);

            await Shell.Current.GoToAsync($"{nameof(WorkerDetailsPage2)}");
        }

        public void CategoryInit()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>
            {
                "5 Stars",
                "4 Stars",
                "3 Stars",
                "2 Stars",
                "1 Stars",
                "All",
            };
            Category = temp;
            SelectedCategory = "All";
        }

        public ObservableCollection<string> Category { get; set; }
        public string SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
        public decimal Minimum { get => minimum; set => SetProperty(ref minimum, value); }
        public decimal Maximum { get => maximum; set => SetProperty(ref maximum, value); }
    }
}