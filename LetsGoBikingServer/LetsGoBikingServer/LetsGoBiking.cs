using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LetsGoBikingServer
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class LetsGoBiking : ILetsGoBiking
    {
        private static readonly HttpClient Client = new HttpClient();

        private static readonly string JCDECEAUX_API_KEY = "8863c86d9599db3b8533179acd4d9ad54d52f975";
        /*public string GetItinerary(string origin, string destination)
        {
            return callAPI();
        */

        public string GetItinerary(string origin, string destination)
        {
            return CallApi();
        }

        private static string CallApi()
        {
            var response = Client
                .GetAsync("http://nominatim.openstreetmap.org/search?q=135+pilkington+avenue,+birmingham&format=json")
                .Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JArray.Parse(responseBody);
            Console.WriteLine(jsonParsed.ToString());
            return jsonParsed.ToString();
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null) throw new ArgumentNullException("composite");
            if (composite.BoolValue) composite.StringValue += "Suffix";
            return composite;
        }


        public GeoCoordinate[] GetClosestStation(Address origin, Address destination)
        {
            // on assume que si on arrive ici, la ville a déjà été vérifiée et donc pas besoin de rappeler l'API
            // on récupère la liste des stations de la ville
            var stations = GetStations(origin.City);

            // check station size
            if (stations.Count == 0) throw new Exception("No station found");

            var closest = new GeoCoordinate[2];
            
            closest[0] = stations[0].geoCoordinate;
            closest[1] = stations[0].geoCoordinate;
            foreach (var station in stations)
            {
                if (station.geoCoordinate.GetDistanceTo(origin.geoCoordinate) < closest[0].GetDistanceTo(origin.geoCoordinate))
                {
                    closest[0] = station.geoCoordinate;
                }
                if (station.geoCoordinate.GetDistanceTo(destination.geoCoordinate) < closest[1].GetDistanceTo(destination.geoCoordinate))
                {
                    closest[1] = station.geoCoordinate;
                }
            }
            
            Console.WriteLine("Closest station to origin: " + closest[0]);
            Console.WriteLine("Closest station to destination: " + closest[1]);
            return closest;
        }

        private static List<Address> GetStations(string city)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + JCDECEAUX_API_KEY).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JArray.Parse(responseBody);
            return jsonParsed.Select(sta => new Address(city,
                new GeoCoordinate((double)sta["position"]["latitude"], (double)sta["position"]["longitude"]))).ToList();
        }
    }
}