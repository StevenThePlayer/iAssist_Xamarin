using iAssist_Xamarin.Models;
using iAssist_Xamarin.ViewModels;
using iAssist_Xamarin.Services;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using iAssist_Xamarin.Views;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class EditTaskViewModel : IAddressAutoComplete
    {
        private MyTaskModel taskData = new MyTaskModel();
        private List<SkillServiceTask> serviceData = new List<SkillServiceTask>();
        private UploadFileServices fileServices = new UploadFileServices();

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
        public Command PageAppearingCommand { get; }
        public Command EditTaskCommand { get; }
        public EditTaskViewModel()
        {
            Title = "Create Task";
            Message = "";
            IsBusy = true;
            IsNotBusy = !IsBusy;
            IsJobSelected = false;

            CurrDate = DateTime.Now;

            uploadFileServices = new UploadFileServices();

            UploadFileCommand = new Command(OnUploadFileClicked);

            ServiceLists = new ObservableCollection<ServiceList>();
            JobCategory = new ObservableCollection<string>();
            PageAppearingCommand = new Command(OnAppearing);
            EditTaskCommand = new Command(OnEditClicked);

            GetBalance();
            IsBusy = false;
            IsNotBusy = !IsBusy;
        }
        public async void OnAppearing()
        {
            taskData = DataKeepServices.GetMyTaskData();
            serviceData = DataKeepServices.GetServiceskData();

            Picture = fileServices.ConvertImageUrl(taskData.TaskImage);
            TaskTitle = taskData.taskdet_name;
            TaskDescription = taskData.taskdet_desc;
            Address = taskData.Loc_Address;
            SelectedJob = taskData.Jobname;
            SelectedDate = taskData.taskdet_sched;
            TaskServices taskServices = new TaskServices();
            createTaskViewModel = await taskServices.GetCreateTask(taskData.jobid.ToString());
            if (createTaskViewModel.SkillList != null)
            {
                ServiceLists.Clear();
                foreach (var data in createTaskViewModel.SkillList)
                {
                    ServiceLists.Add(new ServiceList { ServiceName = data.Skillname });
                }
            }
            foreach (var data in serviceData)
            {
                SelectedServices.Add(new ServiceList { ServiceName = data.Skillname });
            }
            GetBalance();
        }

        public async void OnEditClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (string.IsNullOrWhiteSpace(TaskTitle))
            {
                Message = "Task Title cannot be empty.";
            }
            else if (string.IsNullOrWhiteSpace(TaskDescription))
            {
                Message = "Task Description cannot be empty.";
            }
            else if (string.IsNullOrWhiteSpace(TempAddress.Address))
            {
                Message = "Address cannot be empty.";
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
                bool success = await taskServices.PostEditTask(TaskTitle, TaskDescription, SelectedDate, TempAddress.Address, latitude, longitude, pictureName, services, createTaskViewModel);

                Message = taskServices.Message;
                IsBusy = false;
                IsNotBusy = !IsBusy;

                await Shell.Current.GoToAsync($"//{nameof(MyTaskPage)}");
                await Shell.Current.DisplayAlert(Title, Message, "Ok");
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

        public string Message { get => message; set => SetProperty(ref message, value); }

        public IList<object> SelectedServices { get => selectedServices; set => SetProperty(ref selectedServices, value); }
        public string SelectedJob { get => selectedJob; set => SetProperty(ref selectedJob, value); }

        public string TaskTitle { get => taskTitle; set => SetProperty(ref taskTitle, value); }
        public string TaskDescription { get => taskDescription; set => SetProperty(ref taskDescription, value); }

        public DateTime SelectedDate { get => selectedDate; set => SetProperty(ref selectedDate, value); }
        public DateTime CurrDate { get => currDate; set => SetProperty(ref currDate, value); }
    }
}
