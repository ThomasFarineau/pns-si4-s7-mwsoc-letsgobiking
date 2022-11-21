using System.Device.Location;

namespace LetsGoBikingServer
{
    public class Address
    {
        private string city;
        private GeoCoordinate geoCoordinate;
        
        public Address(string city, GeoCoordinate geoCoordinate)
        {
            this.city = city;
            this.geoCoordinate = geoCoordinate;
        }
    }
}