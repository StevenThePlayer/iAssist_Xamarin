using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    //Create Task
    public class TaskDetailsModel
    {
        public int Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDesc { get; set; }
        public DateTime taskdet_sched { get; set; }
        public DateTime taskdet_Created_at { get; set; }
        public DateTime taskdet_Updated_at { get; set; }
        public int JobId { get; set; }
        public IEnumerable<JobListModel> JobList { get; set; }
        public IEnumerable<SkillListModel> SkillList { get; set; }
        public IEnumerable<string> SelectedSkills { get; set; }
        public string UserId { get; set; }
        public string TaskImage { get; set; }
        public string Address { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public object ImageFile { get; set; }
        public int? workerid { get; set; }
    }

    public class JobListModel
    {
        public int Id { get; set; }
        public string JobName { get; set; }
    }

    public class SkillListModel
    {
        public int Id { get; set; }
        public string Skillname { get; set; }
    }

    public class ServiceList
    {
        public string ServiceName { get; set; }
    }

    //MyTask
    public class taskViewPost
    {
        public List<TaskPostListView> Taskpostlistview { get; set; }
        public List<SkillServiceTask> TaskViewPost { get; set; }
    }

    public class TaskPostListView
    {
        public int Id { get; set; }
        public int Taskbook_Status { get; set; }
        public string taskdet_name { get; set; }
        public string taskdet_desc { get; set; }
        public DateTime taskdet_sched { get; set; }
        public DateTime taskdet_Created_at { get; set; }
        public DateTime taskdet_Updated_at { get; set; }
        public string TaskImage { get; set; }
        public string Loc_Address { get; set; }
        //public DbGeography Geolocation { get; set; }
        public string Jobname { get; set; }
        public int jobid { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int? workerid { get; set; }
        public int? bid { get; set; }
        public int? taskedstatus { get; set; }
        public decimal? taskedTaskPayable { get; set; }
        public string taskedWorkerfname { get; set; }
        public string taskedWorkerlname { get; set; }
        public int? taskedid { get; set; }
        public string Tasktype { get; set; }
        public int? specificworkerid { get; set; }
    }

    public class SkillServiceTask
    {
        public int Id { get; set; }
        public string Skillname { get; set; }
        public int Taskdet { get; set; }
        public string UserId { get; set; }
        public int Jobid { get; set; }
    }

    //My Task Display Collection View
    public class MyTaskModel
    {
        public int Id { get; set; }
        public int Taskbook_Status { get; set; }
        public string taskdet_name { get; set; }
        public string taskdet_desc { get; set; }
        public DateTime taskdet_sched { get; set; }
        public string TaskImage { get; set; }
        public string Loc_Address { get; set; }
        public string Jobname { get; set; }
        public int jobid { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int? workerid { get; set; }
        public int? bid { get; set; }
        public int? taskedstatus { get; set; }
        public decimal? taskedTaskPayable { get; set; }
        public string taskedWorkerfname { get; set; }
        public string taskedWorkerlname { get; set; }
        public int? taskedid { get; set; }
        public string Tasktype { get; set; }
        public int? specificworkerid { get; set; }
        public string ServicesCombined { get; set; }
        public bool IsPending { get; set; }
        public bool IsPosted { get; set; }
        public bool IsOngoing { get; set; }
        public bool IsToComplete { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsToRate { get; set; }
        public bool IsBidded { get; set; }
        public string StatusDisplay { get; set; }
    }

    //Schedule
    public class TaskScheduleModel
    {
        public int EventID { get; set; }
        public int TaskId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string ThemeColor { get; set; }
        public bool IsFullDay { get; set; }
    }
}