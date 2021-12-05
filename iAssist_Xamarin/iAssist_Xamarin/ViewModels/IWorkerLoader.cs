using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using iAssist_Xamarin.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class IWorkerLoader : ViewModelBase
    {
        public List<SearchWorkerModel> workerList;
        private ObservableRangeCollection<SearchWorkerModel> WorkerList { get; set; }

        public AsyncCommand RefreshCommand { get; }
        public Command PageAppearing { get; }

        public IWorkerLoader()
        {
            RefreshCommand = new AsyncCommand(Refresh);
            PageAppearing = new Command(OnAppearing);

            WorkerList = new ObservableRangeCollection<SearchWorkerModel>();
            workerList = new List<SearchWorkerModel>();
        }

        public virtual void OnAppearing()
        {
            GetBalance();
            GetWorkers();
        }

        public virtual async Task Refresh()
        {
            IsBusy = true;

            GetWorkers();

            IsBusy = false;
        }

        public virtual void GetWorkers(){ }

        public void LoadAdapter()
        {
            Load();
        }

        public virtual ObservableRangeCollection<SearchWorkerModel> Load()
        {
            UploadFileServices fileServices = new UploadFileServices();
            if (WorkerList != null)
                WorkerList.Clear();


            if (workerList != null)
            {
                foreach (SearchWorkerModel data in workerList)
                {
                    if (string.IsNullOrWhiteSpace(data.Profile))
                    {
                        data.Profile = "defaultprofilepic.jpg";
                    }
                    else
                    {
                        data.Profile = fileServices.ConvertImageUrl(data.Profile);
                    }
                    WorkerList.Add(data);
                }
            }
            return WorkerList;
        }
    }
}
