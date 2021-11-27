using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class ITaskLoader : ViewModelBase
    {
        public taskViewPost MyTaskViewModelData;


        public AsyncCommand RefreshCommand { get; }
        public Command CategorySelectedCommand { get; }
        public Command PageAppearing { get; }

        private ObservableRangeCollection<MyTaskModel> TaskList { get; set; }
        private ObservableRangeCollection<Grouping<string, MyTaskModel>> TaskListGroups { get; set; }

        private bool isPending, isPosted, isCompleted, isOngoing, isToComplete ,isCanceled, isToRate, isBidded;

        private ObservableCollection<string> Category { get; set; }

        private string servicesCombined, selectedCategory, statusdisplay;
        private int categoryIndex;


        public ITaskLoader()
        {
            TaskList = new ObservableRangeCollection<MyTaskModel>();
            TaskListGroups = new ObservableRangeCollection<Grouping<string, MyTaskModel>>();
            MyTaskViewModelData = new taskViewPost();

            RefreshCommand = new AsyncCommand(Refresh);
            CategorySelectedCommand = new Command(LoadGroupAdapter);
            PageAppearing = new Command(OnAppearing);

            Category = new ObservableCollection<string>();
        }

        public virtual void OnAppearing()
        {
            GetBalance();
            GetTask();
        }

        public virtual async Task Refresh()
        {
            IsBusy = true;

            SelectedCategory = "All";
            GetTask();

            IsBusy = false;
        }

        public virtual void GetTask() { }


        public virtual void SetTaskData(MyTaskModel task)
        {
            DataKeepServices.SetMyTaskData(task);
            DataKeepServices.SetTaskRawData(MyTaskViewModelData.Taskpostlistview.FirstOrDefault(x => x.Id == task.Id));
            DataKeepServices.SetServicesData(MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == task.Id).ToList());
            DataKeepServices.SetTaskId(task.Id);
        }

        public virtual void LoadGroupAdapter()
        {
            LoadGroup();
        }

        public virtual ObservableRangeCollection<Grouping<string, MyTaskModel>> LoadGroup()
        {
            if (MyTaskViewModelData == null)
                return new ObservableRangeCollection<Grouping<string, MyTaskModel>>();

            if (MyTaskViewModelData.Taskpostlistview == null)
                return new ObservableRangeCollection<Grouping<string, MyTaskModel>>();

            if (TaskList != null)
                TaskList.Clear();

            if (TaskListGroups != null)
                TaskListGroups.Clear();

            if (servicesCombined != null)
                servicesCombined = "";

            UploadFileServices fileServices = new UploadFileServices();

            //"Pending", Id = 0
            //"Posted", Id = 1
            //"Ongoing", Id = 2
            //"Completed", Id = 3
            //"Employer Cancelled", Id = 4
            //"Worker Cancelled", Id = 5
            //"Bid Cancelled", Id = 6

            foreach (var data in MyTaskViewModelData.Taskpostlistview)
            {
                isPending = false;
                isCompleted = false;
                isPosted = false;
                isOngoing = false;
                isCanceled = false;
                isToRate = false;
                isToComplete = false;
                servicesCombined = "";

                if (data.Taskbook_Status == 0 || data.Taskbook_Status == 5)
                {
                    isPending = true;
                }
                if (data.Taskbook_Status == 1)
                {
                    isPosted = true;
                }
                if (data.taskedstatus == 4)
                {
                    isToComplete = true;
                }
                if ((data.Tasktype == "" && data.taskedstatus == 5) || (data.Tasktype == null && data.taskedstatus == 5))
                {
                    isToRate = true;
                }
                if (data.Taskbook_Status == 2)
                {
                    isOngoing = true;
                }
                if (data.Taskbook_Status == 3)
                {
                    isCompleted = true;
                }
                if (data.Taskbook_Status == 4)
                {
                    isCanceled = true;
                }
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

                //if (data.Taskbook_Status != 4 && data.Taskbook_Status != 3 && data.taskedstatus != 2 && data.taskedstatus != 3 && data.taskedstatus != 4 && isPending == false && isPosted == false) //taskedstatus 2 means working and 3 completed

                foreach (var data2 in MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == data.Id))
                {
                    servicesCombined += data2.Skillname + ", ";
                }
                if (servicesCombined.Length > 0)
                {
                    var length = servicesCombined.Length - 1;
                    //servicesCombined.Remove(length, 2); //Removes ","
                }
                string picture = fileServices.ConvertImageUrl(data.TaskImage);

                TaskList.Add(new MyTaskModel
                {
                    Id = data.Id,
                    Taskbook_Status = data.Taskbook_Status,
                    taskdet_name = data.taskdet_name,
                    taskdet_desc = data.taskdet_desc,
                    taskdet_sched = data.taskdet_sched,
                    TaskImage = picture,
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
                    IsPending = isPending,
                    IsPosted = isPosted,
                    IsOngoing = isOngoing,
                    IsToComplete = isToComplete,
                    IsCompleted = isCompleted,
                    IsCanceled = isCanceled,
                    IsToRate = isToRate,
                    StatusDisplay = statusdisplay
                });
            }

            //if(TaskListGroups != null)
            //    TaskListGroups.Clear();
            IEnumerable<MyTaskModel> dataList = default;
            IEnumerable<MyTaskModel> dataGroup = default;


            if (SelectedCategory == "All")
            {
                foreach (string cat in Category)
                {
                    if (cat == "Pending")
                    {
                        dataGroup = TaskList.Where(x => x.IsPending);
                    }
                    else if (cat == "Posted")
                    {
                        dataGroup = TaskList.Where(x => x.IsPosted);
                    }
                    else if (cat == "Ongoing")
                    {
                        dataGroup = TaskList.Where(x => x.IsOngoing || x.IsToComplete);
                    }
                    else if (cat == "Completed")
                    {
                        dataGroup = TaskList.Where(x => x.IsCompleted || x.IsToRate);
                    }
                    else if (cat == "Cancelled")
                    {
                        dataGroup = TaskList.Where(x => x.IsCanceled);
                    }

                    if (cat != "All")
                    {
                        if (dataGroup.FirstOrDefault() != null)
                        {
                            TaskListGroups.Add(new Grouping<string, MyTaskModel>(cat, dataGroup));
                        }
                    }
                }
            }
            else
            {

                if (SelectedCategory == "Pending")
                {
                    dataList = TaskList.Where(x => x.IsPending);
                }
                else if (SelectedCategory == "Posted")
                {
                    dataList = TaskList.Where(x => x.IsPosted);
                }
                else if (SelectedCategory == "Ongoing")
                {
                    dataList = TaskList.Where(x => x.IsOngoing || x.IsToComplete);
                }
                else if (SelectedCategory == "Completed")
                {
                    dataList = TaskList.Where(x => x.IsCompleted || x.IsToRate);
                }
                else if (SelectedCategory == "Cancelled")
                {
                    dataList = TaskList.Where(x => x.IsCanceled);
                }

                if (SelectedCategory != null || dataList != null)
                    TaskListGroups.Add(new Grouping<string, MyTaskModel>(SelectedCategory, dataList));
            }
            return TaskListGroups;
        }

        public virtual ObservableRangeCollection<MyTaskModel> Load()
        {
            if (MyTaskViewModelData == null)
                return new ObservableRangeCollection<MyTaskModel>();

            if (TaskList != null)
                TaskList.Clear();

            if (servicesCombined != null)
                servicesCombined = string.Empty;

            UploadFileServices fileServices = new UploadFileServices();

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
                foreach (var data2 in services)
                {
                    servicesCombined += data2.Skillname + ", ";
                }
                if (servicesCombined.Length > 0)
                {
                    var length = servicesCombined.Length - 1;
                    //servicesCombined.Remove(length, 2); //Removes ","
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
        }

        public virtual ObservableCollection<string> CategoryInit()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>
            {
                "All",
                "Pending",
                "Posted",
                "Ongoing",
                "Completed",
                "Cancelled"
            };
            SelectedCategory = "All";
            Category = temp;
            return Category;
        }

        public int CategoryIndex { get => categoryIndex; set => SetProperty(ref categoryIndex, value); }
        public string SelectedCategory { get => selectedCategory; set => SetProperty(ref selectedCategory, value); }
    }
}
