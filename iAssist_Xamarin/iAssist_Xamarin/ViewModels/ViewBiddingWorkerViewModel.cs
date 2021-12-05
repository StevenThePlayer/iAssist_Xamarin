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
    public class ViewBiddingWorkerViewModel : ViewBiddingViewModel
    {
        private BidServices bidServices;
        //private List<BidModel> bidList;

        public AsyncCommand<BidModel> EditBidCommand { get; }
        public AsyncCommand<BidModel> CancelBidCommand { get; }


        public ViewBiddingWorkerViewModel()
        {

            Title = "Task Bids from You";

            //BidList = new ObservableRangeCollection<BidModel>();
            //SortBy = new ObservableCollection<string>();

            bidServices = new BidServices();

            EditBidCommand = new AsyncCommand<BidModel>(OnEditBid);
            CancelBidCommand = new AsyncCommand<BidModel>(OnCancelBid);

            //SortByBidInit();
        }

        public override async void GetBids()
        {
            int id = DataKeepServices.GetTaskId();
                bidList = new List<BidModel>();

                bidServices = new BidServices();

            bidList = await bidServices.GetViewBidding(id, "", (int)2);

            Load();
        }

        public async Task OnEditBid(BidModel bid)
        {
            if (bid == null)
                return;

            DataKeepServices.SetBidData(bid);

            await Shell.Current.GoToAsync($"{nameof(EditBidPage)}");
        }

        
        public override async Task OnCancelBid(BidModel bid)
        {
            if (bid == null)
                return;

            DisplaySelect("Cancel Bid?", $"Cancel Bid for Task: {bid.Tasktitle}", "Task Bid Canceled", "Cancel Failed", bidServices.CancelBid, bid.Bidid, bid.TaskdetId);

            await Shell.Current.GoToAsync($"{nameof(ViewUserRequestTaskPage)}");
        }
    }
}