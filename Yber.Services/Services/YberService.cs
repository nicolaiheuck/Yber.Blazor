using System.Globalization;
using Yber.Repositories.Entities;
using Yber.Repositories.Interfaces;
using Yber.Services.DTO;
using Yber.Services.Interfaces;

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
}