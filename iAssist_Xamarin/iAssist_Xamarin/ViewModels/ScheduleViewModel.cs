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
    public class ScheduleViewModel : ViewModelBase
    {
        private TaskServices taskServices;
        private List<TaskScheduleModel> taskList;

        public ObservableRangeCollection<TaskScheduleModel> TaskList { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public Command SortByCommand { get; }
        public Command PageAppearing { get; }

        public AsyncCommand<TaskScheduleModel> DetailsCommand { get; }

        public ScheduleViewModel()
        {
            Title = "Task Schedule";

            TaskList = new ObservableRangeCollection<TaskScheduleModel>();
            SortBy = new ObservableCollection<string>();

            taskServices = new TaskServices();

            RefreshCommand = new AsyncCommand(Refresh);
            SortByCommand = new Command(Load);
            PageAppearing = new Command(OnAppearing);

            DetailsCommand = new AsyncCommand<TaskScheduleModel>(OnDetailsClicked);

            SortByBidInit();
            GetTask();
        }

        public void OnAppearing()
        {
            GetTask();
            GetBalance();
        }

        public async Task Refresh()
        {
            IsBusy = true;

            GetTask();

            IsBusy = false;
        }

        public virtual async void GetTask()
        {
            taskList = await taskServices.GetSchedule();
            Load();
        }

        public void Load()
        {
            UploadFileServices fileServices = new UploadFileServices();

            if (TaskList != null)
                TaskList.Clear();

            if (taskList != null)
            {
                foreach (var data in taskList)
                {
                    if(data.Start.AddDays(28) >= DateTime.UtcNow)
                    {
                        TaskList.Add(data);
                    }
                }
            }
        }
        public virtual async Task OnDetailsClicked(TaskScheduleModel task)
        {
            if (task == null)
                return;

            DataKeepServices.SetTaskId(task.TaskId);

            await Shell.Current.GoToAsync($"{nameof(ContractTaskPage)}");
        }
    }
}