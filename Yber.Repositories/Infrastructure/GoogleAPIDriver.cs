using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;
using Yber.Repositories.Entities;
using Yber.Repositories.Interfaces;

namespace Yber.Repositories.Infrastructure;

public class GoogleAPIDriver : IGoogleAPIDriver
{
    private async Task<string> GetCalculatedRouteFromGoogleAsync(double[] LatLong)
    {
        DirectionsRequest request = new DirectionsRequest();
        request.Key = "AIzaSyD9q6OWPdId9uZhNOeRYADWyxREdQvXesg";

        request.Origin = new LocationEx(new CoordinateEx(LatLong[0], LatLong[1]));
        request.Destination = new LocationEx(new CoordinateEx(54.909290, 9.799820));

        var response = await GoogleApi.GoogleMaps.Directions.QueryAsync(request);
        
        return response.Routes.First().OverviewPath.Points;
    }
        
    public async Task<Polyline> GetPolylineAsync(double[] LatLong)
    {
        var polyline = new Polyline();
        polyline.EncodedPolyline = await GetCalculatedRouteFromGoogleAsync(LatLong);
        return polyline;
    }
}