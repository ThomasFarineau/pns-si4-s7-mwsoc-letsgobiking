using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace ProxyService
{
    internal static class Program
    {
       private static void Main()
        {
            var httpUrl = new Uri("http://localhost:8733/Design_Time_Addresses/LetsGoBikingServer/ProxyService");
            var Host = new ServiceHost(typeof(ProxyService), httpUrl);

            Host.AddServiceEndpoint(typeof(IProxyService), new BasicHttpBinding(), "");

            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                HttpsGetEnabled = true,
            };
            Host.Description.Behaviors.Add(smb);
            Host.Open();

            Console.WriteLine("ProxyService is running");
            Console.WriteLine("Press <Enter> key to exit" + Environment.NewLine);
            Console.ReadLine();
        }
    }
}
