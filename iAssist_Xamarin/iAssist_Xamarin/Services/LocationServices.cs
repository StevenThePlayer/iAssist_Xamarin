using iAssist_Xamarin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace iAssist_Xamarin.Services
{
    public class LocationServices : GetWithCachingServices
    {
        private readonly string ApiKey = "AIzaSyCCnmMReV03W96lvqJVwbG7nFOTkuUDaX8";
        private readonly string MapAutocompleteUrl;
        private readonly string MapDetailsUrl;

        private List<MapAutocomplete> Addresses = new List<MapAutocomplete>();
        private List<MapAutocomplete> AddressList = new List<MapAutocomplete>();

        public LocationServices()
        {
            MapAutocompleteUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?language=en&types=geocode&region=ph&key=" + ApiKey + "&input=";
            MapDetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json?fields=formatted_address,name,geometry&key=" + ApiKey + "&place_id=";
        }

        public async Task<List<MapAutocomplete>> GetAddress(string input)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, MapAutocompleteUrl + input);

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                if (Addresses != null)
                    Addresses.Clear();
                var output = JsonConvert.DeserializeObject<RootAutocomplete>(content);
                output.predictions.ForEach(x =>
                {
                    MapAutocomplete address = new MapAutocomplete
                    {
                        description = x.description,
                        place_id = x.place_id
                    };
                    Addresses.Add(address);
                }
                );
                return Addresses;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<IList<string>> GetAddressOnly(string EntryAddress, IList<string> Addresses)
        {
            TempAddress.Address = EntryAddress;
            if (AddressList.Count > 0)
            {
                MapAutocomplete temp = AddressList.FirstOrDefault(x => x.description == TempAddress.Address);
                if (temp != null)
                {
                    TempAddress.place_id = temp.place_id;
                }
            }
            if (TempAddress.Address.Length > 2)
            {
                AddressList.Clear();
                AddressList = await GetAddress(TempAddress.Address);
                List<string> data = new List<string>();
                data.Clear();
                AddressList.ForEach(x => data.Add(x.description));
                Addresses = data;


                MapAutocomplete temp = AddressList.FirstOrDefault(x => x.description == TempAddress.Address);
                if (temp != null)
                {
                    TempAddress.place_id = temp.place_id;
                }
            }
            return Addresses;
        }

        public async Task<MapDetails> GetAddressDetails(string input)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, MapDetailsUrl + input);

                var client = new HttpClient();
                var response = await client.SendAsync(request);

                var content = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<RootDetails>(content);
                MapDetails AddressDetails = new MapDetails()
                {
                    lat = output.result.geometry.location.lat.ToString(),
                    lng = output.result.geometry.location.lng.ToString()
                };
                return AddressDetails;
            }
            catch (Exception ex)
            {
                SetMessage("An Error has occurred when communicating with the server.");
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
