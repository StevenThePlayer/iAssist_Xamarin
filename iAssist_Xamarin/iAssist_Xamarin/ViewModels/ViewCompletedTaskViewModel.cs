using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iAssist_Xamarin.ViewModels
{
    public class ViewCompletedTaskViewModel : ContractTaskViewModel
    {
        public ViewCompletedTaskViewModel()
        {
            Title = "My Completed Task";
        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.ViewCompletedTask();
            TaskList = Load();
            /*
            if (TaskList != null)
                TaskList.Clear();

            foreach (var data in list.ToList())
            {
                TaskList.Add(data);
            }*/
        }
    }
}
