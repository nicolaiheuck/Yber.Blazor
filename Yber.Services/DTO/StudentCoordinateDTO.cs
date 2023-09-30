namespace Yber.Services.DTO;

public class StudentCoordinateDTO
{
    public string name { get; set; }
    public latlng latlng { get; set; }
} 
public class latlng
{
    public double lat { get; set; }
    public double lng { get; set; }
}