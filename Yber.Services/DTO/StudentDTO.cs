namespace Yber.Services.DTO;

public class StudentDTO
{
    public string First_Name { get; set; }
    public double[] LatLng { get; set; }
    public bool? Lift_Take { get; set; }
    public bool? Lift_Give { get; set; }
    public string? Username { get; set; }
}