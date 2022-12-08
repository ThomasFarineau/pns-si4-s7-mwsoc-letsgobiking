using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Web.UI.WebControls;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using LetsGoBikingServer.ProxyService;
using Newtonsoft.Json.Linq;

namespace LetsGoBikingServer;

public class LetsGoBiking : ILetsGoBiking
{
    private static readonly HttpClient Client = new();
    private static readonly HttpClient InstructionsClient = new();
    private static readonly string ORS_API_KEY = "5b3ce3597851110001cf6248579351f552544be8b47824b1ed2034c5";

    public string[] GetItinerary(string origin, string destination)
    {
        Console.WriteLine("GetItinerary called with origin: " + origin + " and destination: " + destination);

        var o = CallOrsSearchApi(origin);
        var d = CallOrsSearchApi(destination);
        var instructions = GetInstructions(o, d);
        HandleActiveMQ(instructions);
        return instructions;
    }

    private static string[] GetInstructions(Pair<GeoCoordinate, string> origin, Pair<GeoCoordinate, string> destination)
    {
        var ClosestStationToOrigin = GetClosestStation(origin);
        var ClosestStationToDestination = GetClosestStation(destination);
        var instructions = new List<string>();

        Pair<List<Instruction>, List<GeoCoordinate>> STEP_1 = GetInstructions("foot-walking", origin.First, ClosestStationToOrigin);
        Pair<List<Instruction>, List<GeoCoordinate>> STEP_2 = GetInstructions("cycling-regular", ClosestStationToOrigin, ClosestStationToDestination);
        Pair<List<Instruction>, List<GeoCoordinate>> STEP_3 = GetInstructions("foot-walking", ClosestStationToDestination, destination.First);

        STEP_1.First.ForEach(i => instructions.Add(i.ToString(STEP_1.Second)));
        STEP_2.First.ForEach(i => instructions.Add(i.ToString(STEP_2.Second)));
        STEP_3.First.ForEach(i => instructions.Add(i.ToString(STEP_3.Second)));


        return instructions.ToArray();
    }

    private static Pair<List<Instruction>, List<GeoCoordinate>> GetInstructions(string profile, GeoCoordinate origin, GeoCoordinate destination)
    {
        var instructions = new List<Instruction>();
        var coordinates = new List<GeoCoordinate>();
        InstructionsClient.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
        var Response = InstructionsClient.GetAsync("https://api.openrouteservice.org/v2/directions/" + profile +
                                                   "?api_key=" + ORS_API_KEY + "&start=" + GetCoord(origin) + "&end=" +
                                                   GetCoord(destination)).Result;
        Response.EnsureSuccessStatusCode();
        var responseBody = Response.Content.ReadAsStringAsync().Result;
        var jsonParsed = JObject.Parse(responseBody);
        var features = JObject.Parse(responseBody).GetValue("features")[0];
        var propert = JObject.Parse(features.ToString()).GetValue("properties");
        var segments = JObject.Parse(propert.ToString()).GetValue("segments")[0];
        var steps = JObject.Parse(segments.ToString()).GetValue("steps");
        var geo = JObject.Parse(features.ToString()).GetValue("geometry");
        var coords = JObject.Parse(geo.ToString()).GetValue("coordinates").ToArray();
        foreach(JToken v in coords)
        {
            coordinates.Add(new GeoCoordinate((double)v[0], (double)v[1]));
        }
        foreach (JObject step in steps)
        {
            var text = step.GetValue("instruction").ToString();
            var distance = step.GetValue("distance").ToString();
            var way_points = step.GetValue("way_points").ToArray();
            instructions.Add(new Instruction(text, distance, (int) way_points[0], (int) way_points[1]));
        }
        return new Pair<List<Instruction>, List<GeoCoordinate>>(instructions, coordinates);
    }

    public static string GetCoord(GeoCoordinate g)
    {
        return g.Longitude.ToString().Replace(',', '.') + "," + g.Latitude.ToString().Replace(",", ".");
    }

    private static Pair<GeoCoordinate, string> CallOrsSearchApi(string addr)
    {
        Client.DefaultRequestHeaders.Add("User-Agent", "LetsGoBikingProject");
        var response = Client
            .GetAsync(
                "https://api.openrouteservice.org/geocode/search?api_key=" + ORS_API_KEY + "&text=" +
                addr).Result;
        response.EnsureSuccessStatusCode();
        var responseBody = response.Content.ReadAsStringAsync().Result;
        var jsonParsed = JObject.Parse(responseBody);
        var features = JObject.Parse(responseBody).GetValue("features")[0];
        var propert = JObject.Parse(features.ToString()).GetValue("properties");
        var geometry = JObject.Parse(features.ToString()).GetValue("geometry");
        var ville = JObject.Parse(propert.ToString()).GetValue("locality");
        var coord = JObject.Parse(geometry.ToString()).GetValue("coordinates");
        var geoCoordinate = new GeoCoordinate((double)coord[1], (double)coord[0]);
        return new Pair<GeoCoordinate, string>(geoCoordinate, ville.ToString());
    }

    public static GeoCoordinate GetClosestStation(Pair<GeoCoordinate, string> addr)
    {
        var proxyClient = new ProxyServiceClient();
        var closest = proxyClient.ClosestStation(addr.First, addr.Second);
        return closest.Coordinate;
    }

    public void HandleActiveMQ(string[] instructions)
    {
        // Create a Connection Factory.
        var connecturi = new Uri("activemq:tcp://localhost:61616");
        var connectionFactory = new ConnectionFactory(connecturi);

        // Create a single Connection from the Connection Factory.
        var connection = connectionFactory.CreateConnection();
        connection.Start();

        // Create a session from the Connection.
        var session = connection.CreateSession();

        // Use the session to target a queue.
        IDestination destination = session.GetQueue("instructions");

        // Create a Producer targetting the selected queue.
        var producer = session.CreateProducer(destination);

        // You may configure everything to your needs, for instance:
        producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

        // Finally, to send messages:
        //ITextMessage message = session.CreateTextMessage("Hello World 2");
        //producer.Send(message);

        foreach (var i in instructions)
        {
            var message = session.CreateTextMessage(i);
            producer.Send(message);
        }

        producer.Send(session.CreateTextMessage("END"));

        Console.WriteLine("Message sent, check ActiveMQ web interface to confirm.");
        Console.ReadLine();

        // Don't forget to close your session and connection when finished.
        session.Close();
        connection.Close();
    }

    public class Pair<T, S>
    {
        public Pair(T first, S second)
        {
            First = first;
            Second = second;
        }

        public T First { get; set; }
        public S Second { get; set; }
    }
}