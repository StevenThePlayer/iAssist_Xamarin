using iAssist_Xamarin.Helpers;
using iAssist_Xamarin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class TaskServices : GetWithCachingServices
    {
        private IEnumerable<JobListModel> JobList ;

        public async Task SetJob(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                TempJobCategory.JobName = "";
                TempJobCategory.id = 0;
                return;
            }

            if (JobList == null)
            {
                JobList = await GetJobList();
            }
            else if (JobList.Count() == 0)
            {
                JobList = await GetJobList();
            }
            foreach (JobListModel data in JobList)
            {
                if(jobName.CompareTo(data.JobName) == 0)
                {
                    TempJobCategory.JobName = data.JobName;
                    TempJobCategory.id = data.Id;
                }
            }
        }

        public async Task<IEnumerable<JobListModel>> GetJobList()
        {
            try
            {
                var data = await GetAsync<IEnumerable<JobListModel>>("api/Task/GetJobList", "getjoblist");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<TaskDetailsModel> GetCreateTask(string input)
        {
            try
            {
                string url = "api/Task/GetCreateTaskIndex?JobId=" + input;
                var data = await GetAsync<TaskDetailsModel>(url, "getjoblist");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }


        public async Task<bool> PostCreateTask(
            string taskTitle, string taskDescription, DateTime taskdet_sched, DateTime taskdet_time, string budget, string address, string latitude, string longitude, string image, IEnumerable<string> selectServices, TaskDetailsModel input)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                input.TaskTitle = taskTitle;
                input.TaskDesc = taskDescription;
                input.taskdet_sched = taskdet_sched;
                input.taskdet_time = taskdet_time;
                input.Latitude = latitude;
                input.Longitude = longitude;
                input.TaskImage = image;
                input.SelectedSkills = selectServices;
                input.Address = address;

                if (string.IsNullOrWhiteSpace(budget))
                    input.Budget = 0;
                else
                    input.Budget = decimal.Parse(budget);

                var json = JsonConvert.SerializeObject(input);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Task/PostCreateTaskIndex", httpContent);

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


        public async Task<taskViewPost> GetMyTask(string input)
        {
            try
            {
                string url = "api/Task/ShowMyTaskPost?category=" + input;
                taskViewPost data = await GetAsync<taskViewPost>(url, "getmytask");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new taskViewPost();
            }
        }

        public async Task<bool> PostTask(int id)
        {
            try
            {
                string url = "api/Task/PostTheTask?id=" + id.ToString();
                string data = await GetAsync<string>(url, "posttask");
                return true;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> PostEditTask(int taskId,
            string taskTitle, string taskDescription, DateTime taskdet_sched, DateTime taskdet_time, string budget, string address, string latitude, string longitude, string profilePicture, IEnumerable<string> selectServices, TaskDetailsModel input)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                input.Id = taskId;
                input.TaskTitle = taskTitle;
                input.TaskDesc = taskDescription;
                input.taskdet_sched = taskdet_sched;
                input.taskdet_time = taskdet_time;
                input.Address = address;
                input.Latitude = latitude;
                input.Longitude = longitude;
                input.TaskImage = profilePicture;
                input.SelectedSkills = selectServices;

                if (string.IsNullOrWhiteSpace(budget))
                    input.Budget = 0;
                else
                    input.Budget = decimal.Parse(budget);

                var json = JsonConvert.SerializeObject(input);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Task/EditMyPostTask", httpContent);

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

        public async Task<bool> CancelPostTask(int id, int cancelType = 0)//taskdet, cancelType Seeker/Employer=0, Worker=1
        {
            try
            {
                string url = "api/Task/CancelMyPostTask?id=" + id.ToString() + "&cancel=" + cancelType.ToString();
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
        public async Task<TaskDetailsModel> GetRequestBooking(int inWorkerid, int jobid)
        {
            try
            {
                string url = "api/Task/RequestBooking?inWorkerid=" + inWorkerid.ToString() + "&jobid=" + jobid.ToString();
                TaskDetailsModel data = await GetAsync<TaskDetailsModel>(url, "getrequestbooking");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }
        public async Task<bool> PostRequestBooking(TaskDetailsModel data)
        {
            try
            {
                var client = new HttpClient();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", Settings.AccessToken);

                var json = JsonConvert.SerializeObject(data);

                HttpContent httpContent = new StringContent(json);


                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(
                    Constants.BaseApiAddress + "api/Task/RequestBooking", httpContent);

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

        public async Task<bool> FindWorkerRequestBooking(int id, int task)//id = worker id
        {
            try
            {
                string url = "api/Task/FindWorkerRequestBooking?id=" + id.ToString() + "&task=" + task.ToString();
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

        public async Task<taskViewPost> GetViewUserRequestedTask()
        {
            try
            {
                string url = "api/Task/ViewUserRequestedTask";
                taskViewPost data = await GetAsync<taskViewPost>(url, "getviewuserrequestedtask");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<taskViewPost> GetViewBiddedRequestTask()
        {
            try
            {
                string url = "api/Task/ViewBiddedRequestTask";
                taskViewPost data = await GetAsync<taskViewPost>(url, "getviewbiddedrequesttask");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new taskViewPost();
            }
        }

        public async Task<taskViewPost> GetViewContractTask()
        {
            try
            {
                string url = "api/Task/ViewContractTask";
                taskViewPost data = await GetAsync<taskViewPost>(url, "getviewcontracttask");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new taskViewPost();
            }
        }

        public async Task<bool> MarkasWorking(int id)//tasked
        {
            try
            {
                string url = "api/Task/MarkasWorking?id=" + id.ToString();
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

        public async Task<bool> MarkasDone(int id)//tasked
        {
            try
            {
                string url = "api/Task/MarkasDone?id=" + id.ToString();
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

        public async Task<bool> MarkasCompleteTask(int id)//tasked
        {
            try
            {
                string url = "api/Task/MarkasCompleteTask?id=" + id.ToString();
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

        public async Task<taskViewPost> ViewCompletedTask()
        {
            try
            {
                string url = "api/Task/ViewCompletedTask";
                taskViewPost data = await GetAsync<taskViewPost>(url, "viewcompletedtask");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<TaskScheduleModel>> GetSchedule()
        {
            try
            {
                string url = "api/Task/GetSchedule";
                List<TaskScheduleModel> data = await GetAsync<List<TaskScheduleModel>>(url, "getschedule");
                return data;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return new List<TaskScheduleModel>();
            }
        }
    }
}
