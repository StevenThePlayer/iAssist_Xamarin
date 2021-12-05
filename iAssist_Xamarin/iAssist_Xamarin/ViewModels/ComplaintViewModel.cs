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
    public class ComplaintViewModel : ViewModelBase
    {
        private ComplaintModel complaintData;
        public Command ReportCommand { get; }
        public Command AddImageCommand { get; }

        private UploadFileServices fileServices;
        private string message, workerId, complaintType, description, image;

        public ComplaintViewModel()
        {
            Title = "Report Worker";
            Message = "";
            IsBusy = false;

            fileServices = new UploadFileServices();
            complaintData = new ComplaintModel();

            ReportCommand = new Command(OnReportClicked);
            AddImageCommand = new Command(OnAddImageClicked);
            SetDisplay();
        }

        private async void SetDisplay()
        {
            WorkerId = DataKeepServices.GetWorkerId().ToString();

            complaintData.Workerid = DataKeepServices.GetWorkerId();

            if (string.IsNullOrWhiteSpace(WorkerId))
            {
                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                await Shell.Current.DisplayAlert("Error", "An Error has occured when processing data", "Ok");
            }
        }

        public async void OnReportClicked()
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            if (String.IsNullOrWhiteSpace(ComplaintType))
            {
                Message = "Complaint Type cannot be empty.";
            }
            else if (String.IsNullOrWhiteSpace(Description))
            {
                Message = "Description cannot be empty.";
            }
            else
            {
                IsBusy = true;
                ComplaintServices complaintServices = new ComplaintServices();

                string address = Constants.BaseApiAddress + "api/Upload/";

                string url = await fileServices.UploadFile(address, false);

                bool results = await complaintServices.PostReportWorker(ComplaintType, Description, url, complaintData);


                IsBusy = true;
                IsNotBusy = !IsBusy;
                if (results)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                    await Shell.Current.DisplayAlert("Action Result", $"Worker Reported.", "Ok");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Action Result", $"An error has occured with sender data to the server.", "Ok");
                }
                IsBusy = false;
                IsNotBusy = !IsBusy;
            }
        }


        private async void OnAddImageClicked(object sender)
        {
            Image = await fileServices.SelectFile();
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
        public string ComplaintType
        {
            get => complaintType;
            set => SetProperty(ref complaintType, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
    }
}