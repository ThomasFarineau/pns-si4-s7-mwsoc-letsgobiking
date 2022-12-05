using System.Device.Location;

namespace LetsGoBikingServer;
internal class Instruction
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
}