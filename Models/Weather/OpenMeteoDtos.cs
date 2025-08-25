using System.Text.Json.Serialization;

namespace Integrator.Api.Models.Weather;

internal sealed class GeocodingResponse
{
  [JsonPropertyName("results")] public List<GeocodingResult>? Results { get; set; }
}

internal sealed class GeocodingResult
{
  [JsonPropertyName("name")] public string? Name { get; set; }
  [JsonPropertyName("country")] public string? Country { get; set; }
  [JsonPropertyName("latitude")] public double Latitude { get; set; }
  [JsonPropertyName("longitude")] public double Longitude { get; set; }
}

internal sealed class ForecastResponse
{
  [JsonPropertyName("current_weather")] public CurrentWeather? CurrentWeather { get; set; }
}

internal sealed class CurrentWeather
{
  [JsonPropertyName("temperature")] public double Temperature { get; set; }
  [JsonPropertyName("windspeed")] public double WindSpeed { get; set; }
  [JsonPropertyName("weathercode")] public int WeatherCode { get; set; }
  [JsonPropertyName("time")] public DateTime ObservedAt  { get; set; }
}