using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using LaavorRatingConception;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
namespace iAssist_Xamarin.ViewModels
{
    public class RateWorkerViewModel : ViewModelBase
    {
        private RateModel rateData;
        public Command RateCommand { get; }
        private string workerId, feedback, username, taskid, jobid, message;
        private int rate;

        public RateWorkerViewModel()
        {
            Title = "Rate Worker";
            Message = "";
            IsBusy = false;

            rateData = new RateModel();
            RateCommand = new Command(OnRateClicked);

            GetBalance();
            SetDisplay();
        }

        private async void SetDisplay()
        {
            RateServices rateServices = new RateServices();
            var data = DataKeepServices.GetMyTaskData();

            if (data == null)
            {
                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                await Shell.Current.DisplayAlert("Error", "An Error has occured when processing data", "Ok");
                return;
            }

            if (data.workerid == null)
            {
                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                await Shell.Current.DisplayAlert("Error", "An Error has occured when processing data, missing Id", "Ok");
                return;
            }

            WorkerId = data.workerid.ToString();
            Taskid = data.Id.ToString();
            Jobid = data.jobid.ToString();

            rateData.WorkerId = (int)data.workerid;
            rateData.jobid = data.jobid;
            rateData.taskid = data.Id;

            WorkerId = rateData.WorkerId.ToString();
            Jobid = rateData.jobid.ToString();
            Taskid = rateData.taskid.ToString();

        }

        public async void OnRateClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            if (string.IsNullOrWhiteSpace(Feedback))
            {
                Message = "Feedback cannot be empty.";
            }
            else if (TempRating.Rate == 0)
            {
                Message = "Please give a Rating.";
            }
            else
            {
                IsBusy = true;
                RateServices rateServices = new RateServices();
                bool results = await rateServices.PostCreateFeedbackAndRateWorker(TempRating.Rate, Feedback, rateData);


                IsBusy = true;
                IsNotBusy = !IsBusy;
                if (results)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                    await Shell.Current.DisplayAlert("Action Result", $"Rating and Feedback Given.", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Action Result", $"An error has occured with sender data to the server.", "Ok");
                }
                IsBusy = false;
                IsNotBusy = !IsBusy;
            }
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public string WorkerId
        {
            get => workerId;
            set => SetProperty(ref workerId, value);
        }
        public int Rate
        {
            get => rate;
            set => SetProperty(ref rate, value);
        }
        public string Feedback
        {
            get => feedback;
            set => SetProperty(ref feedback, value);
        }
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        public string Taskid
        {
            get => taskid;
            set => SetProperty(ref taskid, value);
        }
        public string Jobid
{
            get => jobid;
            set => SetProperty(ref jobid, value);
        }
    }
}