using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using Newtonsoft.Json.Linq;

namespace ProxyService;

// REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
public class ProxyService : IProxyService
{
    public static readonly string StationsCacheKey = "stations";
    private static readonly string JCDECEAUX_API_KEY = "8863c86d9599db3b8533179acd4d9ad54d52f975";
    private static readonly int CACHE_EXPIRATION = 15;

    public Station ClosestStation(GeoCoordinate Coordinate, string city)
    {
        var stations = GetStations(city);
        var closest = stations.Select(station => station.Coordinate)
            .OrderBy(station => station.GetDistanceTo(Coordinate)).First();
        var station = stations.Where(stations => stations.Coordinate.Equals(closest)).FirstOrDefault();
        return station;
    }

    public List<Station> GetStations(string City)
    {
        ObjectCache cache = MemoryCache.Default;
        var keyName = StationsCacheKey + "_" + City;

        if (cache.Contains(StationsCacheKey)) return cache.Get(keyName) as List<Station>;
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        var response = client
            .GetAsync("https://api.jcdecaux.com/vls/v1/stations?contract=" + City + "&apiKey=" + JCDECEAUX_API_KEY)
            .Result;
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var jsonParsed = JArray.Parse(responseBody);

        // station list
        var stations = jsonParsed.Select(
            station => new Station((string)station["name"],
                new GeoCoordinate((double)station["position"]["lat"], (double)station["position"]["lng"]))
        ).ToList();
        var cacheItemPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(CACHE_EXPIRATION)
        };
        cache.Add(keyName, stations, cacheItemPolicy);
        return stations;
    }
}