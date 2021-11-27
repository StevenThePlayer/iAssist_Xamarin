using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iAssist_Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {

        ObservableCollection<string> source = new ObservableCollection<string>();

        public RegisterPage()
        {
            InitializeComponent();
            EntryAddress.ItemsSource = source;
        }
        
        private async void AutoCompleteEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            LocationServices locationServices = new LocationServices();
            source.Clear();
            var addresses = await locationServices.GetAddressOnly(EntryAddress.Text, source);
            foreach(var data in addresses)
            {
                source.Add(data);
            }
        }
    }
}