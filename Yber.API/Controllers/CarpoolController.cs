using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Yber.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CarpoolController : ControllerBase
{
    [HttpGet(Name = "GetStudents")]
    public Task<IEnumerable<double[]>> GetStudentLocationsAsync()
    {
        
    }
}