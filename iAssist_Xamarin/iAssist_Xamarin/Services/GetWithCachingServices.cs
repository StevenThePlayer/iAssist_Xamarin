using iAssist_Xamarin.Helpers;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace iAssist_Xamarin.Services
{
    public class GetWithCachingServices
    {
        public HttpClient client = new HttpClient();
        public string Message;

        public GetWithCachingServices()
        {

            try
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri(Constants.BaseApiAddress)
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);
            }
            catch
            {

            }
        }

        public void SetMessage(string message)
        {
            if (message.StartsWith("{"))//Json Message
            {
                JObject jwtDynamic = JsonConvert.DeserializeObject<object>(message) as JObject;
                Message = jwtDynamic.Value<string>("Message");
            }
            else if (message.StartsWith("<"))//Html Message
            {
                // regex which match tags
                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                // replace all matches with empty strin
                Message = rx.Replace(message, "");
            }
            else//string message
            {
                if (string.IsNullOrEmpty(message))
                    Message = "Something went wrong, please try again later.";
                else
                    Message = message;
            }
        }

        public async Task<T> GetAsync<T>(string url, string key, int mins = 1, bool forceRefresh = false)
        {
            T data = default;

            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //    data = Barrel.Current.Get<string>(key);
            //else if (!forceRefresh && !Barrel.Current.IsExpired(key))
            //    data = Barrel.Current.Get<string>(key);

            try
            {
                //if (string.IsNullOrWhiteSpace(data))
                //{
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Constants.BaseApiAddress + url);

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<T>(content);
                }

                //Barrel.Current.Add(key, data, TimeSpan.FromMinutes(mins));
                //}
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("Somethine went wrong, please try again.");

                Debug.WriteLine($"Unable to get information from server {ex}");
                throw ex;
            }
        }


        public async Task<bool> PutAsync(string url)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Constants.BaseApiAddress + url);

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SetMessage("Somethine went wrong, please try again.");

                Debug.WriteLine($"Unable to get information from server {ex}");
                return false;
            }
        }
    }
}
