using iAssist_Xamarin.Models;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace iAssist_Xamarin.ViewModels
{
    public class SearchWorkerViewModel : ViewModelBase
    {
        public Command SearchCommand { get; }
        private string category, address;


        public SearchWorkerViewModel()
        {
            SearchCommand = new Command(OnSearchClicked);
        }

        private async void OnSearchClicked(object obj)
        {
            ;
        }

        public string Category{
            get => category;
            set => SetProperty(ref category, value);
        }

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }
    }
}
