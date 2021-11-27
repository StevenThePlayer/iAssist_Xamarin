using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        private NotificationServices notificationServices;
        private List<NotificationModel> notificationData;

        public ObservableRangeCollection<NotificationModel> NotificationList { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public Command PageAppearing { get; }

        public NotificationsViewModel()
        {
            Title = "Notifications";

            NotificationList = new ObservableRangeCollection<NotificationModel>();

            notificationServices = new NotificationServices();

            RefreshCommand = new AsyncCommand(Refresh);
            PageAppearing = new Command(OnAppearing);
        }

        public void OnAppearing()
        {
            GetBalance();
            GetNotifications();
        }

        NotificationModel selectedNotification;
        public NotificationModel SelectedNotification
        {
            get => selectedNotification;
            set => SetProperty(ref selectedNotification, value);
        }

        public async Task Refresh()
        {
            IsBusy = true;

            GetNotifications();

            IsBusy = false;
        }

        public async void GetNotifications()
        {
            notificationData = await notificationServices.GetNotifications();
            Load();
        }

        public void Load()
        {
            if (notificationData == null)
                return;

            if (NotificationList != null)
                NotificationList.Clear();

            foreach(var data in notificationData)
            {
                NotificationList.Add(data);
            }
        }
    }
}