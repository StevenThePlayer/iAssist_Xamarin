using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace iAssist_Xamarin.ViewModels
{
    public class ContractTaskViewModel : ITaskLoader
    {
        public TaskServices taskServices;

        public ObservableRangeCollection<MyTaskModel> TaskList { get; set; }
        public ObservableRangeCollection<Grouping<string, MyTaskModel>> TaskListGroups { get; set; }

        public AsyncCommand<MyTaskModel> MarkAsWorkingCommand { get; }
        public AsyncCommand<MyTaskModel> MarkAsDoneCommand { get; }
        public AsyncCommand<MyTaskModel> CancelTaskCommand { get; }


        private bool isPending, isPosted, isCompleted, isOngoing, isToComplete, isCanceled, isToRate, isBidded;

        private ObservableCollection<string> Category { get; set; }

        private string servicesCombined, selectedCategory, statusdisplay, image;
        private int categoryIndex;

        public ContractTaskViewModel()
        {
            Title = "My Contract Task";

            taskServices = new TaskServices();

            TaskList = new ObservableRangeCollection<MyTaskModel>();
            TaskListGroups = new ObservableRangeCollection<Grouping<string, MyTaskModel>>();

            MarkAsWorkingCommand = new AsyncCommand<MyTaskModel>(OnMarkAsWorking);
            MarkAsDoneCommand = new AsyncCommand<MyTaskModel>(OnMarkAsDone);
            CancelTaskCommand = new AsyncCommand<MyTaskModel>(OnCancelTask);
            
        }
        
    
        public override ObservableRangeCollection<MyTaskModel> Load()
        {
            UploadFileServices fileServices = new UploadFileServices();
            if (MyTaskViewModelData == null)
                return new ObservableRangeCollection<MyTaskModel>();

            if (TaskList != null)
                TaskList.Clear();

            if (servicesCombined != null)
                servicesCombined = string.Empty;

            foreach (var data in MyTaskViewModelData.Taskpostlistview)
            {
                isOngoing = false;
                isToComplete = false;
                if(data.Taskbook_Status != 4 && data.Taskbook_Status != 5 && data.Taskbook_Status != 3 && data.taskedstatus == 1)
                {
                    isOngoing = true;
                }
                else if (data.taskedstatus != 4)
                {
                    isToComplete = true;
                }
                
                if (data.taskedstatus == 1)
                {
                    statusdisplay = "Worker is ongoing";
                }
                if (data.taskedstatus == 2)
                {
                    statusdisplay = "Cancelled";
                }
                if (data.taskedstatus == 3)
                {
                    statusdisplay = "Worker is working the Task";
                }
                if (data.taskedstatus == 4)
                {
                    statusdisplay = "Worker is done";
                }

                var services = MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == data.Id); 
                servicesCombined = string.Empty;
                foreach (var data2 in services)
                {
                    servicesCombined += data2.Skillname + ", ";
                }
                if (servicesCombined.Length > 0)
                {
                    var length = servicesCombined.Length - 2;
                    servicesCombined.Remove(length, 2); //Removes ","
                }

                isBidded = data.workerid != null;
                image = fileServices.ConvertImageUrl(data.TaskImage);
                TaskList.Add(new MyTaskModel
                {
                    Id = data.Id,
                    Taskbook_Status = data.Taskbook_Status,
                    taskdet_name = data.taskdet_name,
                    taskdet_desc = data.taskdet_desc,
                    taskdet_sched = data.taskdet_sched,
                    TaskImage = image,
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
                    IsOngoing = isOngoing,
                    IsToComplete = isToComplete,
                    StatusDisplay = statusdisplay,
                });
            }
            return TaskList;
        }

        public override void LoadGroupAdapter()
        {
            Load();
        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetViewContractTask();
            LoadGroupAdapter();
        }

        public async Task OnMarkAsWorking(MyTaskModel task)
        {
            if (task == null)
                return;
            
            SetTaskData(task);

            DisplaySelect("Mark Task as working?", $"Mark Task as working: {task.taskdet_name}", "Task Marked as Working", "Action Failed", taskServices.MarkasWorking, (int)task.taskedid);
        }
        public async Task OnMarkAsDone(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            DisplaySelect("Mark Task as done?", $"Mark Task as done: {task.taskdet_name}", "Task Marked as Done", "Action Failed", taskServices.MarkasDone, (int)task.taskedid);
        }

        public async Task OnCancelTask(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            DisplaySelect("Cancel Task?", $"Cancel Task: {task.taskdet_name}", "Task Cancelled", "Task Cancel Failed", taskServices.CancelPostTask, task.Id, 1);
        }
    }
}