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
    public class ComplaintServices : GetWithCachingServices
    {

        public async Task<bool> PostReportWorker(string complaintType, string description, string image, ComplaintModel data)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                data.ComplainType = complaintType;
                data.Description = description;
                data.image = image;

                var json = JsonConvert.SerializeObject(data);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Complaint/ReportWorker", httpContent);

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
    }
}
