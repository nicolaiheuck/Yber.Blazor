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
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return foundStudent ?? new Uber_Students();
    }

    public async Task RequestLift(Uber_Students requester, Uber_Students requestee)
    {
        var request = new Uber_Requests
        {
            RequestApproved = false,
            RequesteeID = requestee.ID,
            RequesterID = requester.ID,
            Requester = null,
            Requestee = null
        };

        _context.Uber_Requests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task ApproveLift(Uber_Students requester, Uber_Students requestee)
    {
        var uberRequest = await _context.Uber_Requests
            .Where(r => r.RequesterID == requester.ID && r.RequesteeID == requestee.ID)
            .FirstOrDefaultAsync();
        uberRequest.RequestApproved = true;
        _context.Uber_Requests.Update(uberRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<Uber_Students> GetStudentFromIdAsync(int studentID)
    {
        var student = await _context.Uber_Students
            .Where(s => s.ID == studentID)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return student;
    }

    public async Task<bool> WantToGetALift(int studentID, bool accept)
    {
        var _student = await _context.Uber_Students
            .FirstOrDefaultAsync(s => s.ID == studentID);
        _student.Lift_Take = accept;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> OfferToDriveAsync(int studentID, bool accept)
    {
        var _student = await _context.Uber_Students.FirstOrDefaultAsync(s => s.ID == studentID);
        _student.Lift_Give = accept;
        try
        { 
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<Uber_Students> FetchActiveRequests(Uber_Students user)
    {
        var foundRequests = await _context.Uber_Students
            .Where(r => r == user)
            .Include(s => s.Requests)
            .FirstOrDefaultAsync();
        return foundRequests ?? new Uber_Students();
    }
}