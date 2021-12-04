using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class FindWorkerViewModel : IWorkerLoader
    {
        private SearchWorkerServices searchWorkerServices;
        private string selectedCategory;

        public Command SortByCommand { get; }

        public ObservableRangeCollection<SearchWorkerModel> WorkerList { get; set; }

        public AsyncCommand<SearchWorkerModel> RequestCommand { get; }
        //public AsyncCommand<BidModel> CancelBiddingCommand { get; }
        public AsyncCommand<SearchWorkerModel> WorkerDetailsCommand { get; }




        public FindWorkerViewModel()
        {
            Title = "Workers for your Task";

            WorkerList = new ObservableRangeCollection<SearchWorkerModel>();
            SortBy = new ObservableCollection<string>();

            searchWorkerServices = new SearchWorkerServices();
            GetWorkers();

            SortByCommand = new Command(LoadAdapter);

            RequestCommand = new AsyncCommand<SearchWorkerModel>(OnRequestClicked);
            WorkerDetailsCommand = new AsyncCommand<SearchWorkerModel>(OnWorkerClicked);
        }

        public override async void GetWorkers()
        {
            workerList = await searchWorkerServices.GetFindWorkerList(DataKeepServices.GetTaskId());

            Load();
        }

        public override ObservableRangeCollection<SearchWorkerModel> Load()
        {
            UploadFileServices fileServices = new UploadFileServices();

            if (WorkerList != null)
                WorkerList.Clear();


            if (workerList != null)
            {
                if (SelectedCategory == "Rating")
                {
                    workerList.OrderByDescending(x => x.Rate);
                }
                foreach (SearchWorkerModel data in workerList)
                {
                    if (string.IsNullOrWhiteSpace(data.Profile))
                    {
                        data.Profile = Constants.BaseApiAddress + "image/defaultprofilepic.jpg";
                    }
                    else
                    {
                        data.Profile = fileServices.ConvertImageUrl(data.Profile);
                    }
                    if(data.UserId != Settings.Email)
                    {
                        WorkerList.Add(data);
                    }
                }
            }

            return WorkerList;
        }
        public async Task OnRequestClicked(SearchWorkerModel worker)
        {
            if (worker == null)
                return;

            DisplaySelect("Request Worker?", $"Request {worker.Lastname}, {worker.Firstname}?", "Request Sent", "Action Failed", searchWorkerServices.FindWorkerRequestBooking, worker.WorkerId, DataKeepServices.GetTaskId());

            await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
        }

        public async Task OnWorkerClicked(SearchWorkerModel worker)
        {
            if (worker == null)
                return;

            DataKeepServices.SetWorkerData(worker);

            await Shell.Current.GoToAsync($"{nameof(WorkerDetailsPage)}");
        }
        public void CategoryInit()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>
            {
                "Distance",
                "Rating",
            };
            Category = temp;
            SelectedCategory = "Distance";
        }

        public ObservableCollection<string> Category { get; set; }
        public string SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
    }
}