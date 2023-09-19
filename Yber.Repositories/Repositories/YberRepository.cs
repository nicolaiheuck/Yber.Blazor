using Microsoft.EntityFrameworkCore;
using Yber.Repositories.DBContext;
using Yber.Repositories.Interfaces;

namespace Yber.Repositories.Repositories;

public class YberRepository : IYberRepository
{
    private YberContext _context;
    
    public YberRepository(YberContext yberContext)
    {
        _context = yberContext;
    }

    private async Task<List<double[]>> GetStudentLocationsFromDatabseAsync(bool needLift = false)
    {
        var _studentsNeedLift = await _context.Uber_Stutents.Where(s => s.Lift_Take == needLift)
            .Include(s => s.City).ToListAsync();

        var _locationArray = new List<double[]>();
        foreach (var student in _studentsNeedLift)
        {
            double _long = 0;
            double _lat = 0;
            Double.TryParse(student.Longitude, out _long);
            Double.TryParse(student.Lattitude, out _lat);
            _locationArray.Add(new []{_lat, _long});
        }

        return _locationArray;
    }
    
    public async Task<List<double[]>> GetLiftStudentLocationArrayListAsync()
    {
        return await GetStudentLocationsFromDatabseAsync(true);
    }

    public async Task<List<double[]>> GetDriverStudentLocationArrayListAsync()
    {
        return await GetStudentLocationsFromDatabseAsync();
    }
}