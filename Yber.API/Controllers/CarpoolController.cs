using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Yber.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CarpoolController : ControllerBase
{
    [HttpGet(Name = "GetStudents")]
    public async Task<IEnumerable<double[]>> GetStudentLocationsAsync()
    {
        // Not implemented
        return null;
    }
}