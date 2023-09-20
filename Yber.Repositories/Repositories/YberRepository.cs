using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Yber.Repositories.DBContext;
using Yber.Repositories.Entities;
using Yber.Repositories.Interfaces;

namespace Yber.Repositories.Repositories;

public class YberRepository : IYberRepository
{
    private YberContext _context;
    
    public YberRepository(YberContext yberContext)
    {
        _context = yberContext;
    }

    private async Task<List<Uber_Students>> GetStudentLocationsFromDatabseAsync(bool needLift = false)
    {
        var _students = new List<Uber_Students>();

        if (needLift)
        {
            _students = await _context.Uber_Students
                .Where(s => s.Lift_Take == true)
                .Include(s => s.City)
                .AsNoTracking()
                .ToListAsync();
        }
        
        if (!needLift)
        {
            _students = await _context.Uber_Students
                .Where(s => s.Lift_Give == true)
                .Include(s => s.City)
                .AsNoTracking()
                .ToListAsync();
        }
        
        return _students;
    }
    
    public async Task<List<Uber_Students>> GetLiftStudentLocationArrayListAsync()
    {
        return await GetStudentLocationsFromDatabseAsync(true);
    }

    public async Task<List<Uber_Students>> GetDriverStudentLocationArrayListAsync()
    {
        return await GetStudentLocationsFromDatabseAsync();
    }

    public async Task<double[]> GetStudentLatLangAsync(string studentUserName)
    {
        var _student = await _context.Uber_Students
            .Where(s => s.Username == studentUserName)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        double _long;
        double _lat;
        Double.TryParse(_student.Longitude, CultureInfo.InvariantCulture, out _long);
        Double.TryParse(_student.Lattitude, CultureInfo.InvariantCulture, out _lat);
        
        var _location = new [] { _lat, _long };
        return _location;
    }
}