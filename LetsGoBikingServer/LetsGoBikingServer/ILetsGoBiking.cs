using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LetsGoBikingServer
{
    [ServiceContract]
    public interface ILetsGoBiking
    {
        [OperationContract]
        string GetItinerary(string origin, string destination);
    }
}