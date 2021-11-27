using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using iAssist_Xamarin.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace iAssist_Xamarin
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            TaskCategories.taskCategory = new List<TaskCategory>();
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Pending", Id = 0 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Posted", Id = 1 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Ongoing", Id = 2 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Completed", Id = 3 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Employer Cancelled", Id = 4 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Worker Cancelled", Id = 5 });
            TaskCategories.taskCategory.Add(new TaskCategory { CategoryName = "Bid Cancelled", Id = 6 });

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
