using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LetsGoBikingServer
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class LetsGoBiking : ILetsGoBiking
    {
        private static readonly HttpClient Client = new HttpClient();
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
            var response = Client.GetAsync("http://nominatim.openstreetmap.org/search?q=135+pilkington+avenue,+birmingham&format=json").Result;
            response.EnsureSuccessStatusCode();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var jsonParsed = JArray.Parse(responseBody);
            Console.WriteLine(jsonParsed.ToString());
            return jsonParsed.ToString();
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
