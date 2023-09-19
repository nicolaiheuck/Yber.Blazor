namespace Yber.Repositories.Entities;

public class Uber_Cities
{
    public int Zipcode { get; set; }
    public string City { get; set; }
    
    // Navigation properties
    public ICollection<Uber_Students> Student  { get; set; }
}