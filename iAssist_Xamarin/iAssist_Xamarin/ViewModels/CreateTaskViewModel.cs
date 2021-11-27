using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
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
    public class CreateTaskViewModel : IAddressAutoComplete
    {
        public Command CreateTaskCommand { get; }
        public Command JobSelectedCommand { get; }
        public Command UploadFileCommand { get; }

        private List<ServiceList> serviceLists = new List<ServiceList>();
        public IList<object> selectedServices;
        private List<string> jobCategory = new List<string>();

        public UploadFileServices uploadFileServices;

        public TaskDetailsModel createTaskViewModel = new TaskDetailsModel();

        private string taskTitle, taskDescription, message, selectedJob, picture;
        private DateTime selectedDate, currDate;
        private bool isJobSelected;

        public CreateTaskViewModel()
        {
            Title = "Create Task";
            Message = "";
            IsBusy = true;
            IsNotBusy = !IsBusy;
            IsJobSelected = false;

            CurrDate = DateTime.Now;
            SelectedDate = CurrDate;

            uploadFileServices = new UploadFileServices();

            CreateTaskCommand = new Command(OnCreateClicked);
            JobSelectedCommand = new Command(OnJobSelectedClicked);
            UploadFileCommand = new Command(OnUploadFileClicked);

            ServiceLists = new ObservableCollection<ServiceList>();
            JobCategory = new ObservableCollection<string>();

            GetJob();
            GetBalance();
            IsBusy = false;
            IsNotBusy = !IsBusy;
        }


        public async void GetJob()
        {
            TaskServices taskServices = new TaskServices();
            IEnumerable<JobListModel> jobs = await taskServices.GetJobList();
            if(jobs != null)
            {
                foreach (JobListModel data in jobs)
                {
                    JobCategory.Add(data.JobName);
                }
            }
        }

        public async void OnJobSelectedClicked(object obj)
        {
            IsBusy = true;
            TaskServices taskServices = new TaskServices();
            await taskServices.SetJob(SelectedJob);
            createTaskViewModel = await taskServices.GetCreateTask(TempJobCategory.id.ToString());
            if(createTaskViewModel.SkillList != null)
            {
                ServiceLists.Clear();
                foreach (var data in createTaskViewModel.SkillList)
                {
                    ServiceLists.Add(new ServiceList { ServiceName = data.Skillname });
                }
            }
            TempAddress.Address = createTaskViewModel.Address;
            Address = createTaskViewModel.Address;
            latitude = createTaskViewModel.Latitude;
            longitude = createTaskViewModel.Longitude;

            LocationServices locationServices = new LocationServices();
            Addresses = await locationServices.GetAddressOnly(Address, Addresses);

            IsJobSelected = true;
            IsBusy = false;
            IsNotBusy = !IsBusy;
            Xamarin.Forms.Shell.Current.ForceLayout();
        }

        public async void OnCreateClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (String.IsNullOrWhiteSpace(TaskTitle))
            {
                Message = "Enter a Task Title.";
            }
            else if (String.IsNullOrWhiteSpace(TaskDescription))
            {
                Message = "Enter a Task Description.";
            }
            else if (TaskDescription.Length < 30)
            {
                Message = "Task Description must be atleast 30 characters.";
            }
            else if (String.IsNullOrWhiteSpace(TempJobCategory.JobName))
            {
                Message = "Enter a Job Category.";
            }
            else if (String.IsNullOrWhiteSpace(TempAddress.Address))
            {
                Message = "Enter an Address.";
            }
            else
            {
                IsBusy = true;
                IsNotBusy = !IsBusy;
                var IsAddressValid = await GetLocation();
                if (IsAddressValid == false)
                {
                    IsBusy = false;
                    IsNotBusy = !IsBusy;
                    Message = "Enter a valid Address.";
                    return;
                }
                List<string> services = new List<string>();
                {
                    IEnumerable<string> data = SelectedServices.Select(x => ((ServiceList)x).ServiceName);
                    foreach (string service in data)
                    {
                        services.Add(service);
                    }
                }
                TaskServices taskServices = new TaskServices();
                string address = Constants.BaseApiAddress + "api/Upload";
                string pictureName = await uploadFileServices.UploadFile(address, false);
                bool success = await taskServices.PostCreateTask(TaskTitle, TaskDescription, SelectedDate, TempAddress.Address, latitude, longitude, pictureName, services, createTaskViewModel);

                if(success)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                    await Shell.Current.DisplayAlert("Sucess", "Task Created Successfully.", "Ok");
                }
                Message = taskServices.Message;
                IsBusy = false;
                IsNotBusy = !IsBusy;
            }
        }

        private async void OnUploadFileClicked(object sender)
        {
            string url;
            url = await uploadFileServices.SelectFile();
            if (string.IsNullOrWhiteSpace(url) == false)
            {
                Picture = url;
            }
        }

        public string Picture
        {
            get => picture;
            set => SetProperty(ref picture, value);
        }


        public ObservableCollection<string> JobCategory { get; set; }
        public ObservableCollection<ServiceList> ServiceLists { get; set; }
        public bool IsJobSelected { get => isJobSelected; set => SetProperty(ref isJobSelected, value); }

        public string Message{ get => message; set => SetProperty(ref message, value); }

        public IList<object> SelectedServices{ get => selectedServices; set => SetProperty(ref selectedServices, value);}
        public string SelectedJob{get => selectedJob;set => SetProperty(ref selectedJob, value);}

        public string TaskTitle { get => taskTitle; set => SetProperty(ref taskTitle, value); }
        public string TaskDescription{get => taskDescription;set => SetProperty(ref taskDescription, value);}

        public DateTime SelectedDate{get => selectedDate;set => SetProperty(ref selectedDate, value);}
        public DateTime CurrDate { get => currDate; set => SetProperty(ref currDate, value); }
    }
}
