using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class WorkerDetailsViewModel2 : IWorkerLoader
    {
        private SearchWorkerServices searchWorkerServices;
        private UploadFileServices fileServices;

        public FindWorkerModel findWorkerData;

        public Command RequestWorkerCommand { get; }
        public Command ReportWorkerCommand { get; }

        public List<RateModel> rateList;
        public ObservableRangeCollection<RateModel> RateList { get; set; }

        string profilePicture, lastname, firstname, jobname, overview;

        public WorkerDetailsViewModel2()
        {

            Title = "Worker Details";

            searchWorkerServices = new SearchWorkerServices();
            RateList = new ObservableRangeCollection<RateModel>();
            fileServices = new UploadFileServices();

            RequestWorkerCommand = new Command(OnRequestWorker);
            ReportWorkerCommand = new Command(OnReportWorker);

        }

        public override async void GetWorkers()
        {
            var data = DataKeepServices.GetWorkerData();
            findWorkerData = await searchWorkerServices.GetFindWorkerDetails(data.WorkerId, data.Taskdet);


            if(findWorkerData == null)
            {
                await Shell.Current.DisplayAlert("Error", "An Error has occurred when processing data.", "Ok");
                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                return;
            }
            if (findWorkerData.viewprofile == null)
            {
                await Shell.Current.DisplayAlert("Error", "An Error has occurred when processing data.", "Ok");
                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                return;
            }

            var workerdata = findWorkerData.viewprofile;

            if (string.IsNullOrWhiteSpace(data.Profile))
            {
                data.Profile = "defaultprofilepic.jpg";
            }
            else
            {
                data.Profile = fileServices.ConvertImageUrl(data.Profile);
            }
            Lastname = workerdata.Lastname;
            Firstname = workerdata.Firstname;
            Jobname = workerdata.Jobname;
            Overview = workerdata.worker_overview;

            var ratedata = findWorkerData.rate;

            if (RateList != null)
                RateList.Clear();

            if (ratedata != null)
            {
                foreach (var ratings in ratedata)
                {
                    RateList.Add(ratings);
                }
            }
        }

        public async void OnRequestWorker()
        {
            if (findWorkerData == null)
                return;

            DisplaySelect("Request Worker?", $"Request {findWorkerData.viewprofile.Lastname}, {findWorkerData.viewprofile.Lastname}?", "Request Sent", "Request Failed", searchWorkerServices.FindWorkerRequestBooking, findWorkerData.viewprofile.WorkerId, DataKeepServices.GetTaskId());
        }

       public async void OnReportWorker()
        {
            if (findWorkerData == null)
                return;

            DataKeepServices.SetWorkerId(findWorkerData.viewprofile.WorkerId);

            await Shell.Current.GoToAsync($"{nameof(ComplaintPage)}");
        }

        public string ProfilePicture
        {
            get => profilePicture;
            set => SetProperty(ref profilePicture, value);
        }
        public string Lastname
        {
            get => lastname;
            set => SetProperty(ref lastname, value);
        }

        public string Firstname
        {
            get => firstname;
            set => SetProperty(ref firstname, value);
        }
        public string Jobname
        {
            get => jobname;
            set => SetProperty(ref jobname, value);
        }

        public string Overview
        {
            get => overview;
            set => SetProperty(ref overview, value);
        }
    }
}