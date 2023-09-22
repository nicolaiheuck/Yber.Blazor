namespace Yber.Services.DTO;

public class RequestDTO
{
    public int RequesterID { get; set; }
    public int RequesteeID { get; set; }
    public bool RequestApproved { get; set; }
}