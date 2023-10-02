using System.Globalization;
using Yber.Repositories.Entities;
using Yber.Repositories.Interfaces;
using Yber.Services.DTO;
using Yber.Services.Interfaces;
using RequestDTO = Yber.Services.DTO.RequestDTO;

namespace Yber.Services.Services;

public class YberService : IYberService
{
    private IYberRepository _YberRepository;
    private IGoogleAPIDriver _GoogleAPIRepo;
    
    public YberService(IYberRepository yberRepository, IGoogleAPIDriver googleAPIRepo)
    {
        _YberRepository = yberRepository;
        _GoogleAPIRepo = googleAPIRepo;
    }
    public async Task<List<StudentDTO>> GetStudentDriverLocationInfoAsync()
    {
        return await GetStudentListInfoAsync(false);
    }

    public async Task<List<StudentDTO>> GetStudentRequesterLocationInfoAsync()
    {
        return await GetStudentListInfoAsync(true);
    }

    private async Task<List<StudentDTO>> GetStudentListInfoAsync(bool lift = false)
    {
        var _students = new List<Uber_Students>();
        switch (lift)
        {
            case true:
                _students = await _YberRepository.GetLiftStudentLocationArrayListAsync();
                break;
            case false:
                _students = await _YberRepository.GetDriverStudentLocationArrayListAsync();
                break;
        }

        var result = new List<StudentDTO>();
        foreach (var student in _students)
        {
            double lat;
            double lng;
            Double.TryParse(student.Lattitude, CultureInfo.InvariantCulture, out lat);
            Double.TryParse(student.Longitude, CultureInfo.InvariantCulture, out lng);
            result.Add(new StudentDTO()
            {
                Id = student.ID,
                First_Name = student.Name_First,
                LatLng = new[] {lat, lng},
                Lift_Give = student.Lift_Give,
                Lift_Take = student.Lift_Take,
                Username = student.Username
            });
        }

        return result;
    }

    public async Task<CalculatedRouteDTO> GetEncodedRouteLineAsync(string studentUsername)
    {
        var _latlng = await _YberRepository.GetStudentLatLangAsync(studentUsername);
        var _polyline = await _GoogleAPIRepo.GetPolylineAsync(_latlng);
        
        return new CalculatedRouteDTO { EncodedPolyline = _polyline.EncodedPolyline };
    }

    public async Task<int> RequestLiftFromUser(string requesterUsername, string requesteeUsername)
    {
        var requester = await _YberRepository.GetStudentFromName(requesterUsername);
        var requestee = await _YberRepository.GetStudentFromName(requesteeUsername);
        if ((requestee.Username == null) || (requester.Username == null)) return 0;

        await _YberRepository.RequestLift(requestee, requester);
        return 1;
    }

    public async Task<int> ApproveLiftRequest(string requesterUsername, string requesteeUsername)
    {
        var requester = await _YberRepository.GetStudentFromName(requesterUsername);
        var requestee = await _YberRepository.GetStudentFromName(requesteeUsername);
        if ((requestee.Username == null) || (requester.Username == null)) return 0;
        await _YberRepository.ApproveLift(requester, requestee);
        return 1;
    }

    public async Task<StudentDTO> GetStudentFromIdAsync(int studentID)
    {
        Uber_Students student = await _YberRepository.GetStudentFromIdAsync(studentID);
        
        return student == null ? new StudentDTO() : new StudentDTO
        {
            Id = student.ID,
            First_Name = student.Name_First!,
            Username = student.Username,
            Lift_Give = student.Lift_Give,
            Lift_Take = student.Lift_Take
        };
    }

    public async Task<bool> WantToGetALiftAsync(int studentId, bool accept)
    {
        var result = await _YberRepository.WantToGetALift(studentId, accept);
        return result;
    }
    
    public async Task<bool> OfferToDriveAsync(int studentId, bool accept)
    {
        var result = await _YberRepository.OfferToDriveAsync(studentId, accept);
        return result;
    }

    public async Task<List<RequestDTO>> GetLiftRequests(string requesteeUsername)
    {
        var student = await _YberRepository.GetStudentFromName(requesteeUsername);
        var requests = await _YberRepository.FetchActiveRequests(student);
        var requestDTO = new List<RequestDTO>();
        
        foreach (var req in requests.Requests)
        {
            requestDTO.Add(new RequestDTO
            {
                RequesteeID = (int)req.RequesteeID!,
                RequesterID = (int)req.RequesterID!,
                RequestApproved = (bool)req.RequestApproved!
            });
        }

        return requestDTO;
    }

    public async Task<StudentDTO> GetStudentFromNameAsync(string studentName)
    {
        var student = await _YberRepository.GetStudentFromName(studentName);

        double lat;
        double lng;
        Double.TryParse(student.Lattitude, CultureInfo.InvariantCulture, out lat);
        Double.TryParse(student.Longitude, CultureInfo.InvariantCulture, out lng);
        var studentDTO = new StudentDTO
        {
            Id = student.ID,
            First_Name = student.Name_First,
            LatLng = new double[]
            {
                lat,
                lng
            },
            Lift_Take = student.Lift_Take,
            Lift_Give = student.Lift_Give,
            Username = student.Username
        };

        return studentDTO;
    }
}