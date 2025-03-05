using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HS.Framework.Utils
{
    public class MapParser
    {
        public Address ParseAddress(string addressStr)
        {
            //https://maps.google.com/maps/api/geocode/json?key=AIzaSyB7vZN1vFPJhCa2TjD0YVjagT8XN9VPe6A&sensor=false&address=73301

            var address = new Address();
            address.ZipCode = addressStr.Split(' ').Last();
            var googleStr = "http://maps.google.com/maps/api/geocode/json?sensor=false&address=" + address;
            var result = new System.Net.WebClient().DownloadString(googleStr);
            var resObj = JsonConvert.DeserializeObject<GoogleGeoCodeResponse>(result);

            address.City = resObj.results[0].address_components[1].long_name;
            address.State = resObj.results[0].address_components[2].short_name;
            addressStr = addressStr.Replace(" " + address.City, "").Replace(" " + address.State, "").Replace(" " + address.ZipCode, "");

            address.StreetNumber = addressStr.Split(' ')[0];
            address.StreetName = addressStr.Split(' ')[1];

            return address;
        }
    }
    public class GoogleGeoCodeResponse
    {

        public string status { get; set; }
        public results[] results { get; set; }

    }

    public class results
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
        public address_component[] address_components { get; set; }
    }

    public class geometry
    {
        public string location_type { get; set; }
        public location location { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class address_component
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }

    public class Address {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    } 
}
