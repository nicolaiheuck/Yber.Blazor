using System.Net.Http.Json;
using Yber.Repositories.Entities;
using Yber.Repositories.Interfaces;

namespace Yber.Repositories.Infrastructure;

public class GoogleAPIDriver : IGoogleAPIDriver
{
    private async Task<CalculatedRoute> GetCalculatedRouteFromGoogleAsync(double[] LatLong)
    {
        var _origin = new Origin();
        _origin.location.latLng.longitude = LatLong[0];
        _origin.location.latLng.latitude = LatLong[1];

        var _googleRequest = new GoogleRouteRequest();
        _googleRequest.origin = _origin;

        var _httpClient = new HttpClient();
        var _baseUrl = new Uri("https://routes.googleapis.com/directions/v2:computeRoutes");
        _httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", "AIzaSyD9q6OWPdId9uZhNOeRYADWyxREdQvXesg");
        _httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", "routes.duration,routes.distanceMeters,routes.polyline.encodedPolyline");
        
        var _googleResponse = await _httpClient.PostAsJsonAsync(_baseUrl, _googleRequest);
        
        return await _googleResponse.Content.ReadFromJsonAsync<CalculatedRoute>();
    }
        
    public async Task<Polyline> GetPolylineAsync(double[] LatLong)
    {
        var responseFromGoogle = await GetCalculatedRouteFromGoogleAsync(LatLong);
        return responseFromGoogle.Polyline;
    }
}