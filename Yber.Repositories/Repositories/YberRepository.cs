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

    public async Task<Uber_Students> GetStudentFromName(string studentUserName)
    {
        var foundStudent = await _context.Uber_Students
            .Where(s => s.Username == studentUserName)
            .FirstOrDefaultAsync();
        return foundStudent ?? new Uber_Students();
    }

    public async Task RequestLift(Uber_Students requester, Uber_Students requestee)
    {
        _context.Uber_Requests.Add(new Uber_Requests
        {
            RequestApproved = false,
            Requestee = requestee,
            Requester = requester
        });
        await _context.SaveChangesAsync();
    }

    public async Task ApproveLift(Uber_Students requester, Uber_Students requestee)
    {
        var uberRequest = await _context.Uber_Requests
            .Where(r => r.RequesterID == requester.Id && r.RequesteeID == requestee.Id)
            .FirstOrDefaultAsync();
        uberRequest.RequestApproved = true;
        _context.Uber_Requests.Update(uberRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Uber_Requests>> FetchActiveRequests(Uber_Students user)
    {
        var foundRequests = await _context.Uber_Requests
            .Where(r => r.Requester == user)
            .ToListAsync();
        return foundRequests;
    }
}