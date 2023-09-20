namespace Yber.Repositories.Entities;

public class Uber_Requests
{
    public int? RequesterID { get; set; }
    public int? RequesteeID { get; set; }
    public bool? RequestApproved { get; set; }
    
    // Navigation properties
    public Uber_Students? Requester { get; set; }
    public Uber_Students? Requestee { get; set; }
}