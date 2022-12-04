using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProxyService
{
    [DataContract]
    public class Station
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public GeoCoordinate Coordinate { get; set; }
        [DataMember]
        public string ContractName { get; set; }

        public Station(string name, GeoCoordinate coordinate, string contractName)
        {
            Name = name;
            Coordinate = coordinate;
            ContractName = contractName;
        }

    }
}
