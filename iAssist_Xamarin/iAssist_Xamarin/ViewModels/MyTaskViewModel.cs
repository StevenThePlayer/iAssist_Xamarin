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
    public class MyTaskViewModel : ITaskLoader
    {
        private TaskServices taskServices;

        public ObservableRangeCollection<MyTaskModel> TaskList { get; set; }
        public ObservableRangeCollection<Grouping<string, MyTaskModel>> TaskListGroups { get; set; }

        public ObservableCollection<string> Category { get; set; }

        public AsyncCommand<MyTaskModel> PostTaskCommand { get; }
        public AsyncCommand<MyTaskModel> FindWorkerCommand { get; }
        public AsyncCommand<MyTaskModel> EditTaskCommand { get; }
        public AsyncCommand<MyTaskModel> CancelTaskCommand { get; }
        public AsyncCommand<MyTaskModel> ViewBiddingCommand { get; }
        public AsyncCommand<MyTaskModel> MarkCompletedCommand { get; }
        public AsyncCommand<MyTaskModel> RateWorkerCommand { get; }
        public AsyncCommand<MyTaskModel> DoNotRateWorkerCommand { get; }
        public AsyncCommand<MyTaskModel> ReportWorkerCommand { get; }



        public MyTaskViewModel()
        {

               Title = "My Task";

            taskServices = new TaskServices();
            Category = new ObservableCollection<string>();

            TaskListGroups = new ObservableRangeCollection<Grouping<string, MyTaskModel>>();
            Category = CategoryInit();

            PostTaskCommand = new AsyncCommand<MyTaskModel>(OnPostTask);
            FindWorkerCommand = new AsyncCommand<MyTaskModel>(OnFindWorker);
            EditTaskCommand = new AsyncCommand<MyTaskModel>(OnEditTask);
            CancelTaskCommand = new AsyncCommand<MyTaskModel>(OnCancelTask);
            ViewBiddingCommand = new AsyncCommand<MyTaskModel>(OnViewBidding);
            MarkCompletedCommand = new AsyncCommand<MyTaskModel>(OnMarkComplete);
            RateWorkerCommand = new AsyncCommand<MyTaskModel>(OnRateWorker);
            DoNotRateWorkerCommand = new AsyncCommand<MyTaskModel>(OnNotRateWorker);
            ReportWorkerCommand = new AsyncCommand<MyTaskModel>(OnReportWorker);
            SelectedCategory = "All";
        }

        public override void LoadGroupAdapter()
        {
            var list = LoadGroup();
            if (TaskListGroups != null)
            {
                TaskListGroups.Clear();
            }
            foreach (var data in list.ToList())
            {
                TaskListGroups.Add(data);
            }
        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetMyTask("");
            LoadGroupAdapter();
        }

        async Task OnPostTask(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            DisplaySelect("Post Task?", $"Post Task: {task.taskdet_name}", $"Post Task Successful", $"Post Task Failed", taskServices.PostTask, task.Id);
        }
        
       async Task OnReportWorker(MyTaskModel task)
        {
            if (task == null)
                return;

            if (task.workerid == null)
                return;

            DataKeepServices.SetWorkerId((int)task.workerid);

            await Shell.Current.GoToAsync($"{nameof(ComplaintPage)}");
        }

        async Task OnFindWorker(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(FindWorkerPage)}");
        }

        async Task OnEditTask(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(EditTaskPage)}");
        }

        async Task OnCancelTask(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            DisplaySelect("Cancel Task?", $"Cancel Task: {task.taskdet_name}", $"Task Cancelled", $"Task Cancel Failed", taskServices.CancelPostTask, task.Id, 0);
        }

        public async Task OnViewBidding(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(ViewBiddingPage)}");
        }

        public async Task OnMarkComplete(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            DisplaySelect("Mark Task as Complete?", $"Mark Task as Complete: {task.taskdet_name}", "Task Marked as Complete", "Task Marking as Complete Failed", taskServices.MarkasCompleteTask, task.Id);
        }

        public async Task OnRateWorker(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(RateWorkerPage)}");
        }

        public async Task OnNotRateWorker(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            RateServices rateServices = new RateServices();

            DisplaySelect("Do Not Rate Worker?", $"Do Not Rate Worker: {task.taskedWorkerlname}, {task.taskedWorkerfname}", "Worker not rated.", "Marking to not rate the Worker has Failed.", rateServices.DontRateWorker, task.Id);
        }

    }
}