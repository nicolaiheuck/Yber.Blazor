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
    public async Task<HttpStatusCode> RequestLiftFromStudentAsync([FromServices] IYberService service, string RequesterUserName,
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
            return HttpStatusCode.InternalServerError;
            throw;
        }
    }

    [HttpPost("/AcceptLift")]
    public async Task<HttpStatusCode> ApproveLiftFromUserAsync([FromServices] IYberService service, string RequesterUserName,
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
            return HttpStatusCode.InternalServerError;
            throw;
        }
    }

    [HttpPost("/ViewLifts")]
    public async Task<List<RequestDTO>> GetActiveRequestsAsync([FromServices] IYberService service, string studentName)
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

    [HttpPost("/WantLift")]
    public async Task<HttpStatusCode> WantLiftAsync([FromServices] IYberService service, int studentID, bool accept)
    {
        try
        {
            var result = await service.WantToGetALiftAsync(studentID, accept); // TODO :D 
            return result == false ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "OfferLift failed");
            return HttpStatusCode.InternalServerError;
            throw;
        }
    }
    
    [HttpPost("/OfferDrive")]
    public async Task<HttpStatusCode> OfferDriveAsync([FromServices] IYberService service, int studentID, bool accept)
    {
        try
        {
            var result = await service.OfferToDriveAsync(studentID, accept); // TODO :D 
            return result == false ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "OfferLift failed");
            return HttpStatusCode.InternalServerError;
            throw;
        }
    }

    [HttpPost("/GetStudentsFromID")]
    public async Task<StudentDTO> GetStudentsFromIdAsync([FromServices] IYberService service, int studentID)
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

    [HttpGet("GetStudentFromName")]
    public async Task<StudentDTO> GetStudentFromNameAsync([FromServices] IYberService service, string studentName)
    {
        try
        {
            var student = await service.GetStudentFromNameAsync(studentName);
            return student;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "GetStudentsFromName failed");
            throw;
        }
    }
}