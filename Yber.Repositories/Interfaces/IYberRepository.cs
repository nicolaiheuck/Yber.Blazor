using Yber.Repositories.Entities;

namespace Yber.Repositories.Interfaces;

public interface IYberRepository
{
    public Task<List<Uber_Students>> GetLiftStudentLocationArrayListAsync();
    public Task<List<Uber_Students>> GetDriverStudentLocationArrayListAsync();
    public Task<double[]> GetStudentLatLangAsync(string studentUserName);
    public Task<Uber_Students> GetStudentFromName(string studentUserName);
    public Task RequestLift(Uber_Students requester, Uber_Students requestee);
    public Task ApproveLift(Uber_Students requester, Uber_Students requestee);
    public Task<Uber_Students> FetchActiveRequests(Uber_Students user);
    public Task<Uber_Students> GetStudentFromIdAsync(int studentID);
}