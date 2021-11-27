using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class RateServices : GetWithCachingServices
    {

        public async Task<RateModel> GetCreateFeedbackAndRateWorker(int id, int taskid, int jobid)//worker id
        {
            try
            {
                string url = "api/Feedback/CreateFeedbackAndRateWorker?id=" + id.ToString() + "&taskid=" + taskid.ToString() + "&jobid=" + jobid.ToString();
                RateModel data = await GetAsync<RateModel>(url, "getcreatefeedbackandrateworker");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new RateModel();
            }
        }
        public async Task<bool> PostCreateFeedbackAndRateWorker(int rate, string feedback, RateModel data)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                data.Feedback = feedback;
                data.Rate = rate;

                var json = JsonConvert.SerializeObject(data);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Feedback/CreateFeedbackAndRateWorker", httpContent);

                string content = await response.Content.ReadAsStringAsync();
                SetMessage(content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DontRateWorker(int taskid)
        {
            try
            {
                string url = "api/Feedback/DontRate?taskid=" + taskid.ToString();
                string data = await GetAsync<string>(url, "dontrate");
                return true;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}
