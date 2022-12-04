using LetsGoBikingServer.ProxyService;
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

        private static readonly HttpClient InstructionsClient = new HttpClient();
        private static readonly string ORS_API_KEY = "5b3ce3597851110001cf6248579351f552544be8b47824b1ed2034c5";

        public string[] GetItinerary(string origin, string destination)
        {
            var o = CallOrsSearchApi(origin);
            var d = CallOrsSearchApi(destination);
            var station = GetClosestStation(o, d);
            var instructions = GetInstructions(o, d, station);
            return instructions;
        }

        private static string[] GetInstructions(Address o, Address d, GeoCoordinate[] station)
        {
            List<string> instructions = new List<string>();
            instructions.AddRange(GetInstructionsFromTo(new GeoCoordinate[] {o.geoCoordinate, station[0] }, "foot-walking"));
            instructions.AddRange(GetInstructionsFromTo(new GeoCoordinate[] { station[0], station[1] }, "cycling-regular"));
            instructions.AddRange(GetInstructionsFromTo(new GeoCoordinate[] { station[1], d.geoCoordinate }, "foot-walking"));
            return instructions.ToArray();
        }

        private static List<string> GetInstructionsFromTo(GeoCoordinate[] coordinates, string profile)
        {
            List<string> instructions = new List<string>();
            InstructionsClient.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
            var response = InstructionsClient.GetAsync("https://api.openrouteservice.org/v2/directions/" + profile + "?api_key=" + ORS_API_KEY + "&start=" +
                getLongitudeLatitude(coordinates[0]) + "&end=" + getLongitudeLatitude(coordinates[1])).Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JObject.Parse(responseBody);
            var features = JObject.Parse(responseBody.ToString()).GetValue("features")[0];
            var propert = JObject.Parse(features.ToString()).GetValue("properties");
            var segments = JObject.Parse(propert.ToString()).GetValue("segments")[0];
            var steps = JObject.Parse(segments.ToString()).GetValue("steps");
            var geo = JObject.Parse(features.ToString()).GetValue("geometry");
            var coords = JObject.Parse(geo.ToString()).GetValue("coordinates");
            int i = 0;
            foreach (JObject step in steps)
            {
                var text = step.GetValue("instruction").ToString();
                var distance = step.GetValue("distance").ToString();
                Instruction instruction = new Instruction(text, distance, new
                    GeoCoordinate((double)coords[i][0], (double)coords[i][1]));

                instructions.Add(instruction.ToString());
                i++;
            }
            return instructions;
        } 

        private static string getLongitudeLatitude(GeoCoordinate g)
        {
            return g.Longitude.ToString().Replace(',', '.') + "," + g.Latitude.ToString().Replace(",", ".");
        }

        private static Address CallOrsSearchApi(string addr)
        {
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
        }

        public GeoCoordinate[] GetClosestStation(Address origin, Address destination)
        {
            var proxyClient = new ProxyServiceClient();
            Station closestToOrigin = proxyClient.ClosestStation(origin.geoCoordinate);
            Station closestToDestionation = proxyClient.ClosestStation(destination.geoCoordinate);


            var res = new  GeoCoordinate[2];
            res[0] = closestToOrigin.Coordinate;
            res[1] = closestToDestionation.Coordinate;

            return res;
        }
    }
}