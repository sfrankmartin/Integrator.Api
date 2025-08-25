using System.Net.Http.Json;
using Integrator.Api.Models.Weather;

namespace Integrator.Api.Services;

internal sealed class WeatherService : IWeatherService
{
  private readonly IHttpClientFactory _httpClientFactory;

  public WeatherService(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  public async Task<WeatherSummaryDto?> GetCurrentWeatherAsync(string city, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(city))
    {
      throw new ArgumentException("City is required", nameof(city));
    }

    var geocodingClient = _httpClientFactory.CreateClient("open-meteo-geocoding");
    var encodedCity = Uri.EscapeDataString(city);
    var geoResponse = await geocodingClient.GetFromJsonAsync<GeocodingResponse>($"v1/search?name={encodedCity}&count=1&language=en&format=json", cancellationToken);

    var result = geoResponse?.Results?.FirstOrDefault();
    if (result == null)
    {
      return null;
    }

    var forecastClient = _httpClientFactory.CreateClient("open-meteo-forecast");

    // Using legacy current_weather=true for simplicity
    var forecastUrl = $"v1/forecast?latitude={result.Latitude}&longitude={result.Longitude}&current_weather=true&timezone=auto";
    var forecast = await forecastClient.GetFromJsonAsync<ForecastResponse>(forecastUrl, cancellationToken);
    if (forecast?.CurrentWeather == null)
    {
      return null;
    }

    return new WeatherSummaryDto
    {
      City = result.Name ?? city,
      Country = result.Country,
      Latitude = result.Latitude,
      Longitude = result.Longitude,
      TemperatureC = forecast.CurrentWeather.Temperature,
      WindSpeedKmh = forecast.CurrentWeather.WindSpeed,
      ObservedAt = forecast.CurrentWeather.ObservedAt,
      Description = MapWeatherCodeToDescription(forecast.CurrentWeather.WeatherCode)
    };
  }

  private static string MapWeatherCodeToDescription(int code)
  {
    return code switch
    {
      0 => "Clear sky",
      1 or 2 or 3 => "Partly cloudy",
      45 or 48 => "Fog",
      51 or 53 or 55 => "Drizzle",
      61 or 63 or 65 => "Rain",
      71 or 73 or 75 => "Snow",
      80 or 81 or 82 => "Rain showers",
      95 => "Thunderstorm",
      96 or 99 => "Thunderstorm with hail",
      _ => "Unknown"
    };
  }
}