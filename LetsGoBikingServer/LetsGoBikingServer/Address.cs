using System.Device.Location;
using System.Runtime.Serialization;

namespace LetsGoBikingServer;

internal class Address
{
    internal string city;

    internal GeoCoordinate geoCoordinate;

    public Address(string city, GeoCoordinate geoCoordinate)
    {
        this.city = city;
        this.geoCoordinate = geoCoordinate;
    }

    public override string ToString()
    {
        return city + " " + geoCoordinate.ToString();
    }

    public string City => city;

    public GeoCoordinate GeoCoordinate => geoCoordinate;
}