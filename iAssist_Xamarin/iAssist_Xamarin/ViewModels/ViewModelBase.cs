using iAssist_Xamarin.Services;
using MvvmHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iAssist_Xamarin.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        public ObservableCollection<string> SortBy { get; set; }

        public decimal Balance;
        public string selectedSortBy;
        public int sortByIndex;

        public ViewModelBase(){ }

        public virtual async void GetBalance()
        {
            AccountServices accountServices = new AccountServices();
            Balance = await accountServices.GetBalance();
            Subtitle = "Balance: " + Balance.ToString() + "Php";
        }

        public async void DisplaySelect(string title, string question, string success, string fail, Func<int,int,Task<bool>> function, int input1, int input2)
        {
            bool answer = await Shell.Current.DisplayAlert(title, question, "Yes", "No");
            if (answer)
            {
                bool result = await function(input1, input2);
                DisplaySelectResult(result, success, fail);
            }
        }

        public async void DisplaySelect(string title, string question, string success, string fail, Func<int, Task<bool>> function, int input)
        {
            bool answer = await Shell.Current.DisplayAlert(title, question, "Yes", "No");
            if (answer)
            {
                bool result = await function(input);
                DisplaySelectResult(result, success, fail);
            }
        }
        private async void DisplaySelectResult(bool result, string success, string fail)
        {
            IsBusy = true;
            IsNotBusy = !IsBusy;
            if (result)
            {
                await Shell.Current.DisplayAlert("Action Result", success, "Ok");
            }
            else
            {
                await Shell.Current.DisplayAlert("Action Result", fail, "Ok");
            }
            IsBusy = false;
            IsNotBusy = !IsBusy;

        }

        public void SortByWorkerInit()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>
            {
                "Name (Asc)",
                "Name (Desc)",
                "Price (Asc)",
                "Price (Desc)",
                "Rating (Asc)",
                "Rating (Desc)"
            };
            SortBy = temp;
            SelectedSortBy = "Name (Asc)";
        }

        public void SortByBidInit()
        {
            ObservableCollection<string> temp = new ObservableCollection<string>
            {
                "Name (Asc)",
                "Name (Desc)",
                "Price (Asc)",
                "Price (Desc)"
            };
            SortBy = temp;
            SelectedSortBy = "Name (Asc)";
        }


        public int SortByIndex { get => sortByIndex; set => SetProperty(ref sortByIndex, value); }
        public string SelectedSortBy { get => selectedSortBy; set => SetProperty(ref selectedSortBy, value); }
    }
}
