using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    public class ViewBidRequestViewModel : ContractTaskViewModel
    {
        public AsyncCommand<MyTaskModel> BidDetailsCommand;
        public ViewBidRequestViewModel()
        {
            Title = "Bidded / RequestTask";
            BidDetailsCommand = new AsyncCommand<MyTaskModel>(OnBidDetails);
        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetViewBiddedRequestTask();
            LoadGroupAdapter();
        }

        public async Task OnBidDetails(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(ViewBiddingPage)}");
        }
    }
}
