using System.Device.Location;
using System.Runtime.Serialization;

namespace ProxyService;

[DataContract]
public class Station
{
    [DataMember] public string Name { get; set; }
    [DataMember] public GeoCoordinate Coordinate { get; set; }
    [DataMember] public string ContractName { get; set; }

    public Station(string name, GeoCoordinate coordinate, string contractName)
    {
        Name = name;
        Coordinate = coordinate;
        ContractName = contractName;
    }
}