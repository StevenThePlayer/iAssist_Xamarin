﻿using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    public class ViewBidRequestViewModel : ITaskLoader
    {
        private TaskServices taskServices;
        public ObservableRangeCollection<MyTaskModel> TaskList { get; set; }

        public AsyncCommand<MyTaskModel> BidDetailsCommand { get; }
        public AsyncCommand<MyTaskModel> BidTaskCommand { get; }
        
        public ViewBidRequestViewModel()
        {
            Title = "Bidded / Request Task";
            BidDetailsCommand = new AsyncCommand<MyTaskModel>(OnBidDetails);
            BidTaskCommand = new AsyncCommand<MyTaskModel>(OnBidTask);
            taskServices = new TaskServices();
            TaskList = new ObservableRangeCollection<MyTaskModel>();
        }

        public override async void GetTask()
        {
            MyTaskViewModelData = await taskServices.GetViewBiddedRequestTask();
            LoadGroupAdapter();
        }

        public override void LoadGroupAdapter()
        {
            var list = Load();
            if (TaskList != null)
            {
                TaskList.Clear();
            }
            foreach (var data in list.ToList())
            {
                TaskList.Add(data);
            }
        }

        public async Task OnBidTask(MyTaskModel task)
        {
            if (task == null)
                return;

            DataKeepServices.SetMyTaskData(task);
            DataKeepServices.SetTaskRawData(MyTaskViewModelData.Taskpostlistview.FirstOrDefault(x => x.Id == task.Id));
            DataKeepServices.SetServicesData(MyTaskViewModelData.TaskViewPost.Where(x => x.Taskdet == task.Id).ToList());
            DataKeepServices.SetTaskId(task.Id);

            await Shell.Current.GoToAsync($"{nameof(CreateBidPage)}");
        }

        public async Task OnBidDetails(MyTaskModel task)
        {
            if (task == null)
                return;

            SetTaskData(task);

            await Shell.Current.GoToAsync($"{nameof(ViewBiddingWorkerPage)}");
        }
    }
}
