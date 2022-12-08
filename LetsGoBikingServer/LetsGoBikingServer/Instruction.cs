using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.Serialization;

namespace LetsGoBikingServer;

[DataContract]
public class Instruction
{
    public Instruction(string text, string distance, int waypointOrigin, int waypointDestination)
    {
        Text = text;
        Distance = distance;
        WaypointOrigin = waypointOrigin;
        WaypointDestination = waypointDestination;

    }

    public string Text { get; set; }
    public string Distance { get; set; }

    public int WaypointOrigin { get; set; }
    public int WaypointDestination { get; set; }

    public string ToString(List<GeoCoordinate> coordinates)
    {
        string ToReturn = Text + "§" + Distance;
        for(int i = 0; i < coordinates.Count; i++)
        {
            ToReturn += "§" + LetsGoBiking.GetCoord(coordinates[i]);
        }
        return ToReturn;
    }
}