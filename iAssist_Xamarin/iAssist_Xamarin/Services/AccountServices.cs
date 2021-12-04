using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class AccountServices : GetWithCachingServices
    {


        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var keyValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("username", email),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("grant_type", "password")
                };

                var request = new HttpRequestMessage(HttpMethod.Post, Constants.BaseApiAddress + "Token");

                request.Content = new FormUrlEncodedContent(keyValues);

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();

                JObject jwtDynamic = JsonConvert.DeserializeObject<object>(content) as JObject;

                if (string.IsNullOrEmpty(jwtDynamic.Value<string>("error_description")) == false)
                {
                    SetMessage(jwtDynamic.Value<string>("error_description"));
                    return false;
                }

                var accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
                var accessToken = jwtDynamic.Value<string>("access_token");

                Settings.AccessTokenExpirationDate = accessTokenExpiration;

                Debug.WriteLine(accessTokenExpiration);

                Debug.WriteLine(content);

                Settings.Email = email;
                Settings.Password = password;
                Settings.AccessToken = accessToken;

                SetMessage("Login Successful");

                return true;
            }
            catch(Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> GetRole(string email = "")
        {
            if (string.IsNullOrWhiteSpace(Settings.Email) == false || string.IsNullOrWhiteSpace(email) == false)
            {
                try
                {
                    string url;
                    if (string.IsNullOrWhiteSpace(email) == false)
                    {
                        url = "api/Mobile/Account/Role?email=" + email;
                    }
                    else
                    {
                        url = "api/Mobile/Account/Role?email=" + Settings.Email;
                    }
                    UserRole data = await GetAsync<UserRole>(url, "getrole");
                    Settings.Role = data.Role;
                    Settings.WorkerId = data.WorkerId.ToString();
                    if (string.IsNullOrEmpty(Settings.Role))
                    {
                        SetMessage("");
                        return false;
                    }
                    SetMessage(Settings.Role);
                    return true;
                }
                catch (Exception ex)
                {
                    SetMessage("An Error has occurred when communicating with the server.");
                    Debug.WriteLine(ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RegisterUserAsync(
            string email, string password, string confirmPassword, string firstname, string address, string latitude, string longitude, string lastname, string phoneNumber)
        {
            try
            {
                var client = new HttpClient();

                var model = new RegisterModel
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                    Firstname = firstname,
                    Address = address,
                    Latitude = latitude,
                    Longitude = longitude,
                    Lastname = lastname,
                    PhoneNumber = phoneNumber
                };

                var json = JsonConvert.SerializeObject(model);

                HttpContent httpContent = new StringContent(json);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Mobile/Account/Register", httpContent);

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

        public async Task<ViewProfileModel> GetProfileAsync()
        {
            try
            {
                return await GetAsync<ViewProfileModel>("api/Manage/UserProfile", "getuserprofile");
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<EditProfileModel> GetEditProfileAsync()
        {
            try
            {
                return await GetAsync<EditProfileModel>("api/Manage/EditUserProfile", "getedituserprofile");
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> PostEditProfileAsync(string email, string firstname, string lastname, string phoneNumber, string userid)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                var model = new EditProfileModel
                {
                    Email = email,
                    Firstname = firstname,
                    Lastname = lastname,
                    Phonenumber = phoneNumber,
                    Userid = userid
                };

                var json = JsonConvert.SerializeObject(model);

                HttpContent httpContent = new StringContent(json);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Manage/EditUserProfile", httpContent);

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


        public async Task<bool> PostChangePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                var model = new ChangePasswordModel
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    ConfirmPassword = confirmNewPassword
                };

                var json = JsonConvert.SerializeObject(model);

                HttpContent httpContent = new StringContent(json);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Manage/ChangePassword", httpContent);

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

        public async Task<decimal> GetBalance()
        {
            try
            {
                decimal data = await GetAsync<decimal>("api/Mobile/Account/GetBalance", "getedituserprofile");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return 0;
            }
        }

        public async Task<List<UserAddress>> GetAddress()
        {
            try
            {
                return await GetAsync<List<UserAddress>>("api/Mobile/Account/Address", "getaddress");
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new List<UserAddress>();
            }
        }
    }
}
