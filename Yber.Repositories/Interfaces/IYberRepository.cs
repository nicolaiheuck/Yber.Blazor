using Yber.Repositories.Entities;

namespace Yber.Repositories.Interfaces;

public interface IYberRepository
{
    public Task<List<Uber_Students>> GetLiftStudentLocationArrayListAsync();
    public Task<List<Uber_Students>> GetDriverStudentLocationArrayListAsync();
    public Task<double[]> GetStudentLatLangAsync(string studentUserName);
}