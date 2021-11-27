using iAssist_Xamarin.Models;
using iAssist_Xamarin.Services;
using MvvmHelpers.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iAssist_Xamarin.ViewModels
{
    public class IAddressAutoComplete : ViewModelBase
    {
        public string latitude, longitude, address;
        public Command AddressChangedCommand { get; }

        public IList<string> addresses;
        private List<MapAutocomplete> AutoCompleteData = new List<MapAutocomplete>();

        public IAddressAutoComplete()
        {
            AddressChangedCommand = new Command(OnAddressChanged);
        }

        public async Task<bool> GetLocation()
        {
            try
            {
                LocationServices locationServices = new LocationServices();
                AutoCompleteData = await locationServices.GetAddress(TempAddress.Address);

                MapDetails loc = await locationServices.GetAddressDetails(TempAddress.place_id);
                longitude = loc.lng;
                latitude = loc.lat;
                if (string.IsNullOrEmpty(longitude) || string.IsNullOrEmpty(latitude))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


        private async void OnAddressChanged(object obj)
        {
            LocationServices locationServices = new LocationServices();
            Addresses = await locationServices.GetAddressOnly(Address, Addresses);
        }

        public IList<string> Addresses { get => addresses; set => SetProperty(ref addresses, value); }
        public string Address { get => address; set => SetProperty(ref address, value); }
    }
}
