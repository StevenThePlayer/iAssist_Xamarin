using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class NotificationModel
    {
        public string Details { get; set; }
        public string Title { get; set; }
        public string DetailsURL { get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
    }
}
