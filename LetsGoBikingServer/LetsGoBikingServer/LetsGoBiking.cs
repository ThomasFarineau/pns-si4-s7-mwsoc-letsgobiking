using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;

namespace LetsGoBikingServer
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class LetsGoBiking : ILetsGoBiking
    {
        private static readonly HttpClient Client = new HttpClient();

        private static readonly string JCDECEAUX_API_KEY = "8863c86d9599db3b8533179acd4d9ad54d52f975";

        public string GetItinerary(string origin, string destination)
        {
            Console.WriteLine("test");
            var o = CallOrsSearchApi(origin);
            var d = CallOrsSearchApi(destination);
            return o + "\n" + d;
        }

        private static Address CallOrsSearchApi(string addr)
        {
            /*
            Client.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
            var response = Client
                .GetAsync(
                    "https://api.openrouteservice.org/geocode/search?api_key=5b3ce3597851110001cf6248579351f552544be8b47824b1ed2034c5&text=" +
                    addr).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JObject.Parse(responseBody);
            var features = JObject.Parse(responseBody.ToString()).GetValue("features")[0];
            var propert = JObject.Parse(features.ToString()).GetValue("properties");
            var geometry = JObject.Parse(features.ToString()).GetValue("geometry");
            var ville = JObject.Parse(propert.ToString()).GetValue("locality");
            var coord = JObject.Parse(geometry.ToString()).GetValue("coordinates");
            var geoCoordinate = new GeoCoordinate((double)coord[1], (double)coord[0]);
            var address = new Address(ville.ToString(), geoCoordinate);
            return address;
            */
            return new Address("Paris", new GeoCoordinate(48.856614, 2.3522219));
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
                if (station.geoCoordinate.GetDistanceTo(origin.geoCoordinate) <
                    closest[0].GetDistanceTo(origin.geoCoordinate)) closest[0] = station.geoCoordinate;
                if (station.geoCoordinate.GetDistanceTo(destination.geoCoordinate) <
                    closest[1].GetDistanceTo(destination.geoCoordinate)) closest[1] = station.geoCoordinate;
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
            var response = client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + JCDECEAUX_API_KEY)
                .Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JArray.Parse(responseBody);
            return jsonParsed.Select(sta => new Address(city,
                new GeoCoordinate((double)sta["position"]["latitude"], (double)sta["position"]["longitude"]))).ToList();
        }
    }
}