namespace Integrator.Api.Models.Weather;

public sealed class WeatherSummaryDto
{
  public required string City { get; init; }
  public string? Country { get; init; }
  public double Latitude { get; init; }
  public double Longitude { get; init; }
  public double TemperatureC { get; init; }
  public double WindSpeedKmh { get; init; }
  public string? Description { get; init; }
  public DateTime ObservedAt { get; init; }
}