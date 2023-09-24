using System.Net;
using AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Yber.Services.DTO;
using Yber.Services.Interfaces;

namespace Yber.API.Controllers;

[ApiController]
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

    [HttpPost("/RequestLift")]
    public async Task<HttpStatusCode> RequestLiftFromStudent([FromServices] IYberService service, string RequesterUserName,
        string RequesteeUserName)
    {
        try
        {
            var result = await service.RequestLiftFromUser(RequesterUserName, RequesteeUserName);
            return result == 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RequestLiftFromStudent failed");
            throw;
        }
    }

    [HttpPost("/AcceptLift")]
    public async Task<HttpStatusCode> ApproveLiftFromUser([FromServices] IYberService service, string RequesterUserName,
        string RequesteeUserName)
    {
        try
        {
            var result = await service.ApproveLiftRequest(RequesterUserName, RequesteeUserName);
            return result == 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ApproveLiftFromUser failed");
            throw;
        }
    }

    [HttpPost("/ViewLifts")]
    public async Task<List<RequestDTO>> GetActiveRequests([FromServices] IYberService service, string studentName)
    {
        try
        {
            var requests = await service.GetLiftRequests(studentName);
            return requests;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetActiveRequests failed");
            throw;
        }
    }

    [HttpPost("/GetStudentsFromID")]
    public async Task<StudentDTO> GetStudentsFromID([FromServices] IYberService service, int studentID)
    {
        try
        {
            var student = await service.GetStudentFromIdAsync(studentID);
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetStudentsFromID failed");
            throw;
        }
    }
}