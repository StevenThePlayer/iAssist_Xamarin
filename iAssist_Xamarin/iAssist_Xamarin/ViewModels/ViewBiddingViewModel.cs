using iAssist_Xamarin.Models;
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
        private List<BidModel> bidList;

        public ObservableRangeCollection<BidModel> BidList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public AsyncCommand<object> SelectedCommand { get; }

        public Command SortByCommand { get; }
        public Command PageAppearing { get; }

        public AsyncCommand<BidModel> AcceptBidCommand { get; }
        public AsyncCommand<BidModel> CancelBiddingCommand { get; }
        public AsyncCommand<BidModel> WorkerDetailsCommand { get; }

        public ViewBiddingViewModel()
        {
            Title = "Task Bids from Workers";

            BidList = new ObservableRangeCollection<BidModel>();
            SortBy = new ObservableCollection<string>();

            bidServices = new BidServices();

            RefreshCommand = new AsyncCommand(Refresh);
            SortByCommand = new Command(Load);
            PageAppearing = new Command(OnAppearing);

            AcceptBidCommand = new AsyncCommand<BidModel>(OnAcceptBid);
            CancelBiddingCommand = new AsyncCommand<BidModel>(OnCancelBid);
            WorkerDetailsCommand = new AsyncCommand<BidModel>(OnWorkerClicked);

            SortByBidInit();
            GetBids();
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

        public void Load()
        {
            UploadFileServices fileServices = new UploadFileServices();

            if(BidList != null)
                BidList.Clear();
            
            if(bidList != null)
            {
                foreach (var data in bidList)
                {
                    data.ProfilePicture = fileServices.ConvertImageUrl(data.ProfilePicture);
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
        }
        public virtual async Task OnAcceptBid(BidModel bid)
        {
            if (bid == null)
                return;

            DisplaySelect("Accept Bid", $"Accept Bid: {bid.Bid_Amount} from worker {bid.Lastname}, {bid.Firstname}", "Bid Accepted", "Action Failed", bidServices.AcceptBid, bid.Bidid, bid.TaskdetId);

            await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
        }

        public virtual async Task OnCancelBid(BidModel bid)
        {
            if (bid == null)
                return;

            DisplaySelect("Cancel Bid?", $"Cancel Bid for Task {bid.Tasktitle}", "Task Bid Canceled", "Action Failed", bidServices.CancelBid, bid.Bidid, bid.TaskdetId);

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

            await Shell.Current.GoToAsync($"{nameof(WorkerDetailsPage)}");
        }
    }
}