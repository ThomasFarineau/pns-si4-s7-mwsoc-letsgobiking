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
        static readonly HttpClient client = new HttpClient();
        /*public string GetItinerary(string origin, string destination)
        {
            return callAPI();
        */

        public async Task<string> GetItinerary(string origin, string destination)
        {
            return await callAPI();
        }

        static async Task<string> callAPI()
        {
            HttpResponseMessage response = await client.GetAsync("http://nominatim.openstreetmap.org/search?q=135+pilkington+avenue,+birmingham&format=json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
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
