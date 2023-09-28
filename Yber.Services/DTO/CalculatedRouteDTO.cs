using System.Text.Json.Serialization;

namespace Yber.Services.DTO;

public class CalculatedRouteDTO
{
    [JsonPropertyName("encodedPolyline")]
    public string EncodedPolyline { get; set; }
}