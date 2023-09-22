using System.Net;
using Microsoft.AspNetCore.Mvc;
using Yber.Services.DTO;
using Yber.Services.Interfaces;

namespace Yber.API.Controllers;

[ApiController]
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

    [HttpPost("/RequestLift")]
    public async Task<HttpStatusCode> RequestLiftFromStudent([FromServices] IYberService service, string RequesterUserName,
        string RequesteeUserName)
    {
        var result = await service.RequestLiftFromUser(RequesterUserName, RequesteeUserName);
        return result == 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
    }

    [HttpPost("/AcceptLift")]
    public async Task<HttpStatusCode> ApproveLiftFromUser([FromServices] IYberService service, string RequesterUserName,
        string RequesteeUserName)
    {
        var result = await service.ApproveLiftRequest(RequesterUserName, RequesteeUserName);
        return result == 0 ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
    }

    [HttpPost("/ViewLifts")]
    public async Task<List<RequestDTO>> GetActiveRequests([FromServices] IYberService service, string studentName)
    {
        var requests = await service.GetLiftRequests(studentName);
        return requests;
    }
}