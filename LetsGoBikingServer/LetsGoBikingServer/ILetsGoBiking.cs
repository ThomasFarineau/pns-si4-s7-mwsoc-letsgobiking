using System.ServiceModel;

namespace LetsGoBikingServer;

[ServiceContract]
public interface ILetsGoBiking
{
    [OperationContract]
    string[] GetItinerary(string origin, string destination);
}