using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProxyService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class ProxyService : IProxyService
    {
        public static readonly string StationsCacheKey = "stations";
        private static readonly string JCDECEAUX_API_KEY = "8863c86d9599db3b8533179acd4d9ad54d52f975";
        private static readonly int CACHE_EXPIRATION = 15;

        public Station ClosestStation(GeoCoordinate coordinate)
        {
            var stations = GetStations();
            Station closest = stations[0];
            foreach (Station station in stations)
            {
                if (station.Coordinate.GetDistanceTo(coordinate) <
                   closest.Coordinate.GetDistanceTo(coordinate)) closest = station;
            }
            return closest;
        }

        public List<Station> GetStations()
        {
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(StationsCacheKey))
            {
                return cache.Get(StationsCacheKey) as List<Station>;
            }
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + JCDECEAUX_API_KEY)
                .Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JArray.Parse(responseBody);

            // station list
            var stations = jsonParsed.Select(
                station => new Station((string) station["name"], new GeoCoordinate((double)station["position"]["latitude"], (double)station["position"]["longitude"]), (string) station["contractName"])
                ).ToList();

            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(CACHE_EXPIRATION)
            };
            cache.Add(StationsCacheKey, stations, cacheItemPolicy);
            return stations;
        }
    }
}
