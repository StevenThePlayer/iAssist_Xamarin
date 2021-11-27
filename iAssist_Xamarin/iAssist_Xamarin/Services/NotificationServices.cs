using iAssist_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class NotificationServices : GetWithCachingServices
    {
        public async Task<List<NotificationModel>> GetNotifications()
        {
            try
            {
                string url = "api/Notifications/Notifications";
                var data = await GetAsync<List<NotificationModel>>(url, "getnotifications");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return default;
            }
        }
    }
}
