using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using Newtonsoft.Json.Bson;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace iAssist_Xamarin.Services
{
    public class UploadFileServices
    {
        public FileResult file;
        public async Task<string> UploadFile(string uploadAddress, bool AddBaseAddress = true)
        {
            string url = string.Empty;

            if (file == null)
                return null;

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new StreamContent(await file.OpenReadAsync()), "file", file.FileName);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", Settings.AccessToken);

            var httpResponseMessage = await client.PostAsync(uploadAddress, content);
            var output = await httpResponseMessage.Content.ReadAsStringAsync();
            output = Regex.Replace(output, "[,\"]", string.Empty).Trim();
            if (string.Compare(output, "no files") != 0 && AddBaseAddress)
            {
                url = Constants.BaseApiAddress + "image/" + output;
            }
            else
            {
                url = output;
            }
            return url;
        }

        public async Task<string> SelectFile()
        {
            try
            {
                file = await MediaPicker.PickPhotoAsync();
                if(file != null)
                    return file.FileName;

                return "";
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return "";
            }
        }

        public string ConvertImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return "";

            string output = Regex.Replace(imageUrl, "[,\"~]", string.Empty).Trim();
            if(output[0] == '\\')
                output = output.Remove(0, 1);
            output = Constants.BaseApiAddress + "image/" + output;
            return output;
        }
    }
}
