namespace Yber.Repositories.Entities;

public class CalculatedRoute
{
    public int DistanceMeters { get; set; }
    public string DurationSeconds { get; set; }
    public Polyline Polyline { get; set; }
}

public class Polyline
{
    public string EncodedPolyline { get; set; }
}