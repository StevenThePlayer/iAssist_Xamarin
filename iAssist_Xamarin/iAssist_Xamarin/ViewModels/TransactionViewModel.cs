using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Command = MvvmHelpers.Commands.Command;

namespace iAssist_Xamarin.ViewModels
{
    public class TransactionViewModel : ViewModelBase
    {
        private TransactionServices transactionServices;
        private List<TransactionModel> transactionData;

        public ObservableRangeCollection<TransactionModel> TransactionList { get; set; }

        public AsyncCommand RefreshCommand { get; }

        public Command PageAppearing { get; }

        public TransactionViewModel()
        {
            Title = "Transactions";

            TransactionList = new ObservableRangeCollection<TransactionModel>();

            transactionServices = new TransactionServices();
            GetTransactions();

            RefreshCommand = new AsyncCommand(Refresh);
            PageAppearing = new Command(OnAppearing);
        }

        public void OnAppearing()
        {
            GetTransactions();
            GetBalance();
        }

        public async Task Refresh()
        {
            IsBusy = true;

            GetTransactions();

            IsBusy = false;
        }

        public async void GetTransactions()
        {
            transactionData = await transactionServices.GetTransactions();
            Load();
        }

        public void Load()
        {
            if (transactionData == null)
                return;
            if (TransactionList != null)
                TransactionList.Clear();

            foreach (var data in transactionData)
            {
                TransactionList.Add(data);
            }
        }
    }
}
