using System.Globalization;

namespace Yber.Repositories.Entities;

public class GoogleRouteRequest
{
    public Origin origin { get; set; }
    public Destination destination { get; set; }
    public string travelMode { get; set; } = "DRIVE";
    public string routingPreference { get; set; } = "TRAFFIC_AWARE";
    public string departureTime { get; set; } = DateTime.Now.ToString(CultureInfo.InvariantCulture);
    public bool computeAlternativeRoutes { get; set; } = false;
    public RouteModifiers routeModifiers { get; set; }
    public string languageCode { get; set; } = "en-US";
    public string units { get; set; } = "METRIC";
}

public class Destination
{
    public Location location { get; set; } = new Location()
    {
        latLng = new LatLng()
        {
            longitude = 9.799820, 
            latitude = 54.909290
        }
    };
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