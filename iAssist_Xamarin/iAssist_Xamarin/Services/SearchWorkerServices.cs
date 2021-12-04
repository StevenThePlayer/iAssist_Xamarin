using iAssist_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class SearchWorkerServices : GetWithCachingServices
    {
        public async Task<List<SearchWorkerModel>> GetFindWorkerList(int id, string category = "")
        {
            try
            {
                string url = "api/SearchWorker/FindWorkerList?id=" + id.ToString() + "&category" + category;
                List<SearchWorkerModel> data = await GetAsync<List<SearchWorkerModel>>(url, "getfindworkerlist");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);

                return new List<SearchWorkerModel>();
            }
        }


        public async Task<bool> FindWorkerRequestBooking(int id, int task)
        {
            try
            {
                string url = "api/Task/FindWorkerRequestBooking?id=" + id.ToString() + "&task=" + task.ToString();
                string data = await GetAsync<string>(url, "findworkerrequestbooking");
                return true;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);

                return false;
            }
        }

        public async Task<FindWorkerModel> GetFindWorkerDetails(int id, int task)//Worker id, taskdet
        {
            try
            {
                string url = "api/SearchWorker/ViewDetailsChoosenWorker?id=" + id.ToString() + "&task=" + task.ToString();
                FindWorkerModel data = await GetAsync<FindWorkerModel>(url, "getfindworkerdetails");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);

                return new FindWorkerModel();
            }
        }

    }
}
