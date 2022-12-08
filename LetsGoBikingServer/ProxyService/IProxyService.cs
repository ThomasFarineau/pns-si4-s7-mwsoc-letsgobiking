using System.Collections.Generic;
using System.Device.Location;
using System.ServiceModel;

namespace ProxyService;

// REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
[ServiceContract]
public interface IProxyService
{
    [OperationContract]
    List<Station> GetStations(string city);

    [OperationContract]
    Station ClosestStation(GeoCoordinate Coordinate, string City);
}