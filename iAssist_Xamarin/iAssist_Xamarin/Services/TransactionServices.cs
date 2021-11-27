using iAssist_Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace iAssist_Xamarin.Services
{
    public class TransactionServices : GetWithCachingServices
    {
        public async Task<List<TransactionModel>> GetTransactions()
        {
            try
            {
                string url = "api/Transactions/Transactions";
                var data = await GetAsync<List<TransactionModel>>(url, "gettransactions");
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
