namespace Integrator.Api.Models.Weather;

/// <summary>
/// Simplified weather data returned by our API
/// </summary>
public sealed class WeatherSummaryDto
{
  /// <summary>
  /// Requested city name
  /// </summary>
  public required string City { get; init; }
  /// <summary>
  /// Country name if available
  /// </summary>
  public string? Country { get; init; }
  /// <summary>
  /// Latitude
  /// </summary>
  public double Latitude { get; init; }
  /// <summary>
  /// Longitude
  /// </summary>
  public double Longitude { get; init; }
  /// <summary>
  /// Temperature in Celsius
  /// </summary>
  public double TemperatureC { get; init; }
  /// <summary>
  /// Wind speed in km/h
  /// </summary>
  public double WindSpeedKmh { get; init; }
  /// <summary>
  /// Short description derived from weather code
  /// </summary>
  public string? Description { get; init; }
  /// <summary>
  /// Observation time in provider timezone
  /// </summary>
  public DateTime ObservedAt { get; init; }
}