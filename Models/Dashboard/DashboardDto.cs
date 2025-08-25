using Integrator.Api.Models.News;
using Integrator.Api.Models.Weather;

namespace Integrator.Api.Models.Dashboard;

/// <summary>
/// Aggregated data from News and Weather endpoints 
/// </summary>
public sealed class DashboardDto
{
  public WeatherSummaryDto? Weather { get; init; }
  public IReadOnlyList<NewsItemDto> TopNews { get; init; } = Array.Empty<NewsItemDto>();
}