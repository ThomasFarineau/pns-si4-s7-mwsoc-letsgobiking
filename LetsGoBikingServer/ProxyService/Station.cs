using System.Device.Location;
using System.Runtime.Serialization;

namespace ProxyService;

[DataContract]
public class Station
{
    public Station(string name, GeoCoordinate coordinate)
    {
        Name = name;
        Coordinate = coordinate;
    }

    [DataMember] public string Name { get; set; }
    [DataMember] public GeoCoordinate Coordinate { get; set; }
}