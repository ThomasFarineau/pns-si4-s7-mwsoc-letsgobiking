using System.Device.Location;
using System.Runtime.Serialization;

namespace LetsGoBikingServer
{
    [DataContract(Name = "Address", Namespace = "")]
    public class Address : IExtensibleDataObject
    {
        [DataMember(Name = "City")] internal string city;

        [DataMember(Name = "Coordinate")] internal GeoCoordinate geoCoordinate;

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

        public ExtensionDataObject ExtensionData { get; set; }
    }
}