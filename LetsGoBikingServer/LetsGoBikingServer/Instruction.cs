using System.Device.Location;
using System.Runtime.Serialization;

namespace LetsGoBikingServer;

[DataContract]
public class Instruction
{
    public string Text { get; set; }
    public string Distance { get; set; }

    public GeoCoordinate Location { get; set; }

    public Instruction(string text, string distance, GeoCoordinate location)
    {
        Text = text;
        Distance = distance;
        Location = location;
    }

    public override string ToString() {
        return Text + "§" + Distance + "§" + Location.Longitude + "§" + Location.Latitude;
    }

}