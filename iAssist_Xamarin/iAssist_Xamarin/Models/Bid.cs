using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class BidModel
    {
        public int Bidid { get; set; }
        public decimal Bid_Amount { get; set; }
        public string Bid_Description { get; set; }
        public int TaskdetId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicture { get; set; }
        public string Tasktitle { get; set; }
        public int? user { get; set; }
        public int? workerid { get; set; }
        public string Username { get; set; }
        public int? bookstatus { get; set; }
        public double? Rate { get; set; }
    }
}
