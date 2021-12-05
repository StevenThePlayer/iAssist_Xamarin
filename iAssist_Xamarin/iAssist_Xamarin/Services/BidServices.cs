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
    public class BidServices : GetWithCachingServices
    {
        public async Task<List<BidModel>> GetViewBidding(int id, string category = "", int user = 1)// user = 1 employer, user = 2 worker, id = taskdet
        {
            try
            {
                string url = "api/Bid/ViewBidding?id=" + id.ToString() + "&user=" + user.ToString() + "&category" + category;
                var data = await GetAsync<List<BidModel>>(url, "getviewbidding");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }
        public async Task<bool> AcceptBid(int id, int taskid)// user
        {
            try
            {
                string url = "api/Bid/AcceptBid?id=" + id.ToString() + "&taskid=" + taskid.ToString();
                bool result = await PutAsync(url);
                return result;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CancelBid(int id, int taskid)//bid id
        {
            try
            {
                string url = "api/Bid/CancelBidding?id=" + id.ToString() + "&taskid=" + taskid.ToString();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, Constants.BaseApiAddress + url);

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer", Settings.AccessToken);

                var response = await client.SendAsync(request);

                await GetAsync<string>(url, "cancelbid");
                return true;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<BidModel> GetEditBidding(int id)//bid id
        {
            try
            {
                string url = "api/Bid/EditBidding?id=" + id.ToString();
                return await GetAsync<BidModel>(url, "editbidding");
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new BidModel();
            }
        }

        public async Task<bool> PostEditBidding(decimal bidAmount, string bidDescription, BidModel input)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                input.Bid_Amount = bidAmount;
                input.Bid_Description = bidDescription;

                var json = JsonConvert.SerializeObject(input);

                HttpContent httpContent = new StringContent(json);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Bid/EditBidding", httpContent);

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

        public async Task<bool> PostCreateBidToTheTask(decimal bidAmount, string bidDescription, int id)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                BidModel input = new BidModel
                {
                    TaskdetId = id,
                    Bid_Amount = bidAmount,
                    Bid_Description = bidDescription
                };

                var json = JsonConvert.SerializeObject(input);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Bid/CreateBidToTheTask", httpContent);

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
