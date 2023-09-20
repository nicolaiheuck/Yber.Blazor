using Yber.Repositories.Entities;

namespace Yber.Repositories.Interfaces;

public interface IGoogleAPIDriver
{
    public Task<Polyline> GetPolylineAsync(double[] LatLong);
}