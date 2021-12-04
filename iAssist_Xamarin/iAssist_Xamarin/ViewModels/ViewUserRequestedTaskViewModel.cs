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
        public ObservableRangeCollection<MyTaskModel> TaskList { get; set; }

        public ObservableCollection<string> Category { get; set; }

        public AsyncCommand<MyTaskModel> BidTaskCommand { get; }
        public AsyncCommand<MyTaskModel> DetailsBidCommand { get; }

        public ViewUserRequestedTaskViewModel()
        {

            Title = "User Requested / Posted Task";

            TaskList = new ObservableRangeCollection<MyTaskModel>();
            Category = new ObservableCollection<string>();

            taskServices = new TaskServices();

            GetTask();
            GetBalance();

            BidTaskCommand = new AsyncCommand<MyTaskModel>(OnBidTask);
            DetailsBidCommand = new AsyncCommand<MyTaskModel>(OnDetailsBid);

        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetViewUserRequestedTask();
            LoadGroupAdapter();
        }

        public override void LoadGroupAdapter()
        {
            var list = Load();
            if (TaskList != null)
            {
                TaskList.Clear();
            }
            foreach (var data in list.ToList())
            {
                TaskList.Add(data);
            }
        }

        /*
        public override ObservableRangeCollection<MyTaskModel> Load()
        {
            if (MyTaskViewModelData == null)
                return new ObservableRangeCollection<MyTaskModel>();

            if (TaskList != null)
                TaskList.Clear();

            if (servicesCombined != null)
                servicesCombined = string.Empty;

            UploadFileServices fileServices = new UploadFileServices();

            if(MyTaskViewModelData.Taskpostlistview == null)
                return new ObservableRangeCollection<MyTaskModel>();

            foreach (var data in MyTaskViewModelData.Taskpostlistview)
            {

                statusdisplay = "";
                if (data.Taskbook_Status == 0)//0 means pending and the task can be edit
                {
                    statusdisplay = "Pending";
                }
                if (data.Taskbook_Status == 1)//1 means Posted and can`t be edit
                {
                    statusdisplay = "Posted";
                }
                if (data.Taskbook_Status == 2)//2 means ge sugdan na ang trabaho
                {
                    statusdisplay = "Ongoing";
                }
                if (data.Taskbook_Status == 3)//3 if complete na ang task or mana
                {
                    statusdisplay = "Completed";
                }
                if (data.Taskbook_Status == 4)// 4 if cancelled by the Creator of the task
                {
                    statusdisplay = "User cancelled this Task";
                }
                if (data.Taskbook_Status == 5)// 4 if cancelled by the specific worker
                {
                    statusdisplay = "You cancelled this Task";
                }

                var services = MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == data.Id);
                if(services != null)
                {
                    foreach (var data2 in services)
                    {
                        servicesCombined += data2.Skillname + ", ";
                    }
                    for
                    if (servicesCombined.Length > 0)
                    {
                        string temp = servicesCombined;
                        servicesCombined = temp.Remove(temp.Length - 1, 2);
                        //servicesCombined.Remove(length, 2); //Removes ","
                    }
                }
                isBidded = data.workerid != null;

                data.TaskImage = fileServices.ConvertImageUrl(data.TaskImage);

                TaskList.Add(new MyTaskModel
                {
                    Id = data.Id,
                    Taskbook_Status = data.Taskbook_Status,
                    taskdet_name = data.taskdet_name,
                    taskdet_desc = data.taskdet_desc,
                    taskdet_sched = data.taskdet_sched,
                    TaskImage = data.TaskImage,
                    Loc_Address = data.Loc_Address,
                    Jobname = data.Jobname,
                    jobid = data.jobid,
                    UserId = data.UserId,
                    Username = data.Username,
                    workerid = data.workerid,
                    bid = data.bid,
                    taskedstatus = data.taskedstatus,
                    taskedTaskPayable = data.taskedTaskPayable,
                    taskedWorkerfname = data.taskedWorkerfname,
                    taskedWorkerlname = data.taskedWorkerlname,
                    taskedid = data.taskedid,
                    Tasktype = data.Tasktype,
                    specificworkerid = data.specificworkerid,
                    ServicesCombined = servicesCombined,
                    IsBidded = isBidded,
                    StatusDisplay = statusdisplay,
                });
            }
            return TaskList;
        }*/

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