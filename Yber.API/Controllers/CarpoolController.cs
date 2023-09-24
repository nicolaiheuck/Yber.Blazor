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
    private readonly ILogger<CarpoolController> _logger;

    public CarpoolController(ILogger<CarpoolController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/GetStudentLift")]
    public async Task<List<StudentDTO>> GetStudentLiftLocationsAsync([FromServices] IYberService service)
    {
        try
        {
            var studentLifts = await service.GetStudentRequesterLocationInfoAsync();
            return studentLifts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetStudentLiftLocationsAsync failed");
            throw;
        }
    }

    [HttpGet("/GetStudentDriver")]
    public async Task<List<StudentDTO>> GetStudentDriverLocationsAsync([FromServices] IYberService service)
    {
        try
        {
            var studentDriver = await service.GetStudentDriverLocationInfoAsync();
            return studentDriver;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetStudentDriverLocationsAsync failed");
            throw;
        }
    }

    [HttpPost("/GetStudentRoute")]
    public async Task<CalculatedRouteDTO> GetCurrentStudentRouteInfoAsync([FromServices] IYberService service, string studentName)
    {
        try
        {
            var studentInfo = await service.GetEncodedRouteLineAsync(studentName);
            return studentInfo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetCurrentStudentRouteInfoAsync failed");
            throw;
        }
    }
}