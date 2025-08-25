using Integrator.Api.Models.Weather;

namespace Integrator.Api.Services;

public interface IWeatherService
{
  Task<WeatherSummaryDto?> GetCurrentWeatherAsync(string city, CancellationToken cancellationToken);
}