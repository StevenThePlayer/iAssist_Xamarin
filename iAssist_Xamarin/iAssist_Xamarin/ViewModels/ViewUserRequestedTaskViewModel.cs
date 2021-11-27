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
    public class ViewUserRequestedTaskViewModel : ITaskLoader
    {
        private TaskServices taskServices;

        public ObservableRangeCollection<MyTaskModel> TaskList2 { get; set; }
        public ObservableRangeCollection<Grouping<string, MyTaskModel>> TaskListGroups { get; set; }

        public ObservableCollection<string> Category { get; set; }

        public AsyncCommand<MyTaskModel> BidTaskCommand { get; }

        public ViewUserRequestedTaskViewModel()
        {

            Title = "User Requested / Posted Task";

            TaskList2 = new ObservableRangeCollection<MyTaskModel>();
            Category = new ObservableCollection<string>();

            taskServices = new TaskServices();

            GetTask();
            GetBalance();

            BidTaskCommand = new AsyncCommand<MyTaskModel>(OnBidTask);

        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetViewUserRequestedTask();
            var list = Load();

            if (TaskList2 != null)
                TaskList2.Clear();

            foreach (var data in list.ToList())
            {
                TaskList2.Add(data);
            }
        }


        public async Task OnBidTask(MyTaskModel task)
        {
            if (task == null)
                return;

            DataKeepServices.SetMyTaskData(task);
            DataKeepServices.SetTaskRawData(MyTaskViewModelData.Taskpostlistview.FirstOrDefault(x => x.Id == task.Id));
            DataKeepServices.SetServicesData(MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == task.Id).ToList());
            DataKeepServices.SetTaskId(task.Id);

            await Shell.Current.GoToAsync($"{nameof(CreateBidPage)}");
        }
        public async Task OnDetailsBid(MyTaskModel task)
        {
            if (task == null)
                return;

            DataKeepServices.SetMyTaskData(task);
            DataKeepServices.SetTaskRawData(MyTaskViewModelData.Taskpostlistview.FirstOrDefault(x => x.Id == task.Id));
            DataKeepServices.SetServicesData(MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == task.Id).ToList());
            DataKeepServices.SetTaskId(task.Id);

            await Shell.Current.GoToAsync($"{nameof(ViewBiddingWorkerPage)}");
        }

        public async Task OnCancelTask(MyTaskModel task)
        {
            if (task == null)
                return;

            DisplaySelect("Cancel Task?", $"Cancel Task: {task.taskdet_name}", "Task Cancelled", "Action Failed", taskServices.CancelPostTask, (int)task.taskedid, 1);
        }
    }
}