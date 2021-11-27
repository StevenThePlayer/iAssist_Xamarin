using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class TaskCategories
    {
        public static List<TaskCategory> taskCategory { get; set; }
    }
    public class TaskCategory
    {
        public string CategoryName { get; set; }
        public int Id { get; set; }
    }
}
