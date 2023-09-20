using Yber.Services.DTO;

namespace Yber.Services.Interfaces;

public interface IYberService
{
    public Task<List<StudentDTO>> GetStudentDriverLocationInfoAsync();
    public Task<List<StudentDTO>> GetStudentRequesterLocationInfoAsync();
    public Task<CalculatedRouteDTO> GetEncodedRouteLineAsync(string studentUsername);
}