using System;
using System.Collections.Generic;

namespace iAssist_Xamarin.Models
{
    public class SearchWorkerModel
    {
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int JobId { get; set; }
        public IEnumerable<JobListModel> JobList { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Profile { get; set; }
        public string nearaddress { get; set; }
        public string Jobname { get; set; }
        public string UserId { get; set; }
        public int WorkerId { get; set; }
        public string distance { get; set; }
        public int Taskdet { get; set; }
        public double? Rate { get; set; }
    }

    public class ProfileViewSkilledWorker
    {
        public int WorkerId { get; set; }
        public int Jobid { get; set; }
        public string Userid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicture { get; set; }
        public string worker_overview { get; set; }
        public string Jobname { get; set; }
        public int? taskdet { get; set; }
    }

    public class FindWorkerModel
    {
        public ProfileViewSkilledWorker viewprofile { get; set; }
        public List<RateModel> rate { get; set; }
    }
}