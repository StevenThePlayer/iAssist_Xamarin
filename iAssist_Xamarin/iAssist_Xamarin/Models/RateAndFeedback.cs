using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    //Create Rating (RateandFeedback)
    public class RateModel
    {
        public int WorkerId { get; set; }
        public int Rate { get; set; }
        public string Feedback { get; set; }
        public string Username { get; set; }
        public int? taskid { get; set; }
        public int jobid { get; set; }
    }

    //Show Rating (Rating)
    public class RatingModel
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Feedback { get; set; }
        public string UsernameFeedback { get; set; }
        public int WorkerID { get; set; }
        //public virtual Work Works { get; set; }
        public int Jobid { get; set; }
    }
}
