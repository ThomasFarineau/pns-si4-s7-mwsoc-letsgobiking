using LetsGoBikingServer.ProxyService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace LetsGoBikingServer;

public class LetsGoBiking : ILetsGoBiking
{
    private static readonly HttpClient Client = new();
    private static readonly HttpClient InstructionsClient = new();
    private static readonly string ORS_API_KEY = "5b3ce3597851110001cf6248579351f552544be8b47824b1ed2034c5";

    public string[] GetItinerary(string origin, string destination)
    {
        var o = CallOrsSearchApi(origin);
        var d = CallOrsSearchApi(destination);
        var instructions = GetInstructions(o, d);
        HandleActiveMQ(instructions);
        return instructions;
    }

    private static string[] GetInstructions(GeoCoordinate origin, GeoCoordinate destination)
    {
        var ClosestStationToOrigin = GetClosestStation(origin);
        var ClosestStationToDestination = GetClosestStation(destination);
        var st = new string[2];
        var instructions = new List<string>();
        GetInstructions("foot-walking", origin, ClosestStationToOrigin).ForEach(i => instructions.Add(i.ToString()));
        GetInstructions("cycling-regular", ClosestStationToOrigin, ClosestStationToDestination).ForEach(i => instructions.Add(i.ToString()));
        GetInstructions("foot-walking", ClosestStationToDestination, destination).ForEach(i => instructions.Add(i.ToString()));
        return instructions.ToArray();
    }

    private static List<Instruction> GetInstructions(string profile, GeoCoordinate origin, GeoCoordinate destination)
    {
        var instructions = new List<Instruction>();
        InstructionsClient.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
        var Response = InstructionsClient.GetAsync("https://api.openrouteservice.org/v2/directions/" + profile + "?api_key=" + ORS_API_KEY + "&start=" + GetCoord(origin) + "&end=" + GetCoord(destination)).Result;
        Response.EnsureSuccessStatusCode();
        var responseBody = Response.Content.ReadAsStringAsync().Result;
        var jsonParsed = JObject.Parse(responseBody);
        var features = JObject.Parse(responseBody.ToString()).GetValue("features")[0];
        var propert = JObject.Parse(features.ToString()).GetValue("properties");
        var segments = JObject.Parse(propert.ToString()).GetValue("segments")[0];
        var steps = JObject.Parse(segments.ToString()).GetValue("steps");
        var geo = JObject.Parse(features.ToString()).GetValue("geometry");
        var coords = JObject.Parse(geo.ToString()).GetValue("coordinates");
        var i = 0;
        foreach (JObject step in steps)
        {
            var text = step.GetValue("instruction").ToString();
            var distance = step.GetValue("distance").ToString();
            instructions.Add(new Instruction(text, distance, new
                GeoCoordinate((double)coords[i][0], (double)coords[i][1])));
            i++;
        }

        return instructions;
    }

    private static string GetCoord(GeoCoordinate g)
    {
        return g.Longitude.ToString().Replace(',', '.') + "," + g.Latitude.ToString().Replace(",", ".");
    }

    private static GeoCoordinate CallOrsSearchApi(string addr)
    {
        Client.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
        var response = Client
            .GetAsync(
                "https://api.openrouteservice.org/geocode/search?api_key=" + ORS_API_KEY + "&text=" +
                addr).Result;
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var jsonParsed = JObject.Parse(responseBody);
        var features = JObject.Parse(responseBody.ToString()).GetValue("features")[0];
        var propert = JObject.Parse(features.ToString()).GetValue("properties");
        var geometry = JObject.Parse(features.ToString()).GetValue("geometry");
        var ville = JObject.Parse(propert.ToString()).GetValue("locality");
        var coord = JObject.Parse(geometry.ToString()).GetValue("coordinates");
        var geoCoordinate = new GeoCoordinate((double)coord[1], (double)coord[0]);
        return geoCoordinate;
    }

    public static GeoCoordinate GetClosestStation(GeoCoordinate geoCoordinate)
    {
        var proxyClient = new ProxyServiceClient();
        Station closest = proxyClient.ClosestStation(geoCoordinate);
        return closest.Coordinate;
    }

    public void HandleActiveMQ(string[] instructions)
    {
        // Create a Connection Factory.
        Uri connecturi = new Uri("activemq:tcp://localhost:61616");
        ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

        // Create a single Connection from the Connection Factory.
        IConnection connection = connectionFactory.CreateConnection();
        connection.Start();

        // Create a session from the Connection.
        ISession session = connection.CreateSession();

        // Use the session to target a queue.
        IDestination destination = session.GetQueue("instructions");

        // Create a Producer targetting the selected queue.
        IMessageProducer producer = session.CreateProducer(destination);

        // You may configure everything to your needs, for instance:
        producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

        // Finally, to send messages:
        //ITextMessage message = session.CreateTextMessage("Hello World 2");
        //producer.Send(message);

        foreach (string i in instructions)
        {
            ITextMessage message = session.CreateTextMessage(i);
            producer.Send(message);
        }

        Console.WriteLine("Message sent, check ActiveMQ web interface to confirm.");
        Console.ReadLine();

        // Don't forget to close your session and connection when finished.
        session.Close();
        connection.Close();
    }
}