using System.Globalization;

namespace Yber.Repositories.Entities;

public class GoogleRouteRequest
{
    public Origin origin { get; set; }
    public Destination destination { get; set; } = new Destination();
    public string travelMode { get; set; } = "DRIVE";
    public string routingPreference { get; set; } = "TRAFFIC_AWARE";
    public string departureTime { get; set; } = DateTime.Now.ToUniversalTime().ToString("o");
    public bool computeAlternativeRoutes { get; set; } = false;
    public RouteModifiers routeModifiers { get; set; } = new RouteModifiers();
    public string languageCode { get; set; } = "en-US";
    public string units { get; set; } = "IMPERIAL";
}

public class Destination
{
    public Location location { get; set; } = new Location();
    public Destination()
    {
        location.latLng = new LatLng
        {
            longitude = 9.799820,
            latitude = 54.909290
        };
    }
}

public class LatLng
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}

public class Location
{
    public LatLng latLng { get; set; }
}

public class Origin
{
    public Location location { get; set; }
}

public class RouteModifiers
{
    public bool avoidTolls { get; set; } = false;
    public bool avoidHighways { get; set; } = false;
    public bool avoidFerries { get; set; } = false;
}