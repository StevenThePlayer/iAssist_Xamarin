using iAssist_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Services
{
    public static class DataKeepServices
    {
        private static MyTaskModel MyTaskData { get; set; }
        private static TaskPostListView TaskViewData { get; set; }
        private static List<SkillServiceTask> ServicesData { get; set; }
        private static BidModel BidData { get; set; }
        private static SearchWorkerModel WorkerData { get; set; }
        private static int WorkerId { get; set; }
        private static int TaskId { get; set; }

        public static void SetMyTaskData(MyTaskModel data)
        {
            if (MyTaskData == null)
                MyTaskData = new MyTaskModel();
            MyTaskData = data;
        }

        public static MyTaskModel GetMyTaskData()
        {
            if (MyTaskData != null)
                return MyTaskData;
            else
                return new MyTaskModel();
        }

        public static void SetTaskRawData(TaskPostListView data)
        {
            if(TaskViewData == null)
            {
                TaskViewData = new TaskPostListView();
            }
            TaskViewData = data;
        }


        public static TaskPostListView GetTaskRawData()
        {
            if (TaskViewData != null)
                return TaskViewData;
            else
                return new TaskPostListView();
        }

        public static void SetServicesData(List<SkillServiceTask> data)
        {
            if (ServicesData == null)
            {
                ServicesData = new List<SkillServiceTask>();
            }
            ServicesData = data;
        }


        public static List<SkillServiceTask> GetServiceskData()
        {
            if (ServicesData != null)
                return ServicesData;
            else
                return new List<SkillServiceTask>();
        }
        public static void SetBidData(BidModel data)
        {
            if (BidData == null)
                BidData = new BidModel();
            BidData = data;
        }

        public static BidModel GetBidData()
        {
            if (BidData != null)
                return BidData;
            else
                return new BidModel();
        }

        public static void SetWorkerData(SearchWorkerModel data)
        {
            if (WorkerData == null)
                WorkerData = new SearchWorkerModel();
            WorkerData = data;
        }

        public static SearchWorkerModel GetWorkerData()
        {
            if (WorkerData != null)
                return WorkerData;
            else
                return new SearchWorkerModel();
        }
        public static void SetWorkerId(int data)
        {
            WorkerId = data;
        }

        public static int GetWorkerId()
        {
            return WorkerId;
        }

        public static void SetTaskId(int data)
        {
            TaskId = data;
        }

        public static int GetTaskId()
        {
            return TaskId;
        }
    }
}
