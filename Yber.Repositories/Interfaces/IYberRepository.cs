namespace Yber.Repositories.Interfaces;

public interface IYberRepository
{
    public Task<List<double[]>> GetLiftStudentLocationArrayListAsync();
    public Task<List<double[]>> GetDriverStudentLocationArrayListAsync();
}