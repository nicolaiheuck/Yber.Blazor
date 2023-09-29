using Yber.Services.DTO;

namespace Yber.Services.Interfaces;

public interface IYberService
{
    public Task<List<StudentDTO>> GetStudentDriverLocationInfoAsync();
    public Task<List<StudentDTO>> GetStudentRequesterLocationInfoAsync();
    public Task<CalculatedRouteDTO> GetEncodedRouteLineAsync(string studentUsername);
    public Task<int> RequestLiftFromUser(string requesterUsername, string requesteeUsername);
    public Task<int> ApproveLiftRequest(string requesterUsername, string requesteeUsername);
    public Task<List<RequestDTO>> GetLiftRequests(string requesteeUsername);
    public Task<StudentDTO> GetStudentFromIdAsync(int studentID);
    public Task<bool> OfferToDriveAsync(int studentId, bool accept);
    public Task<bool> WantToGetALiftAsync(int studentId, bool accept);
    public Task<StudentDTO> GetStudentFromNameAsync(string studentName);
}