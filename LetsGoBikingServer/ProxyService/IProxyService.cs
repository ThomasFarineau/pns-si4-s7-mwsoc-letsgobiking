using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProxyService;

// REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
[ServiceContract]
public interface IProxyService
{
    [OperationContract]
    List<Station> GetStations();

    [OperationContract]
    Station ClosestStation(GeoCoordinate coordinate);
}