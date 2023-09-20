using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yber.Services.DTO;
using Yber.Services.Interfaces;

namespace Yber.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class CarpoolController : ControllerBase
{
    [HttpGet("/GetStudentLift")]
    public async Task<List<StudentDTO>> GetStudentLiftLocationsAsync([FromServices] IYberService service)
    {
        var studentLifts = await service.GetStudentRequesterLocationInfoAsync();
        return studentLifts;
    }

    [HttpGet("/GetStudentDriver")]
    public async Task<List<StudentDTO>> GetStudentDriverLocationsAsync([FromServices] IYberService service)
    {
        var studentDriver = await service.GetStudentDriverLocationInfoAsync();
        return studentDriver;
    }

    [HttpPost("/GetStudentRoute")]
    public async Task<CalculatedRouteDTO> GetCurrentStudentRouteInfoAsync([FromServices] IYberService service, string studentName)
    {
        var studentInfo = await service.GetEncodedRouteLineAsync(studentName);
        return studentInfo;
    }
}