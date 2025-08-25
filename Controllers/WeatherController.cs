using Integrator.Api.Models.Weather;
using Integrator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Integrator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class WeatherController : ControllerBase
{
  private readonly IWeatherService _weatherService;
  private readonly ILogger<WeatherController> _logger;

  public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
  {
    _weatherService = weatherService;
    _logger = logger;
  }

  [HttpGet]
  [ProducesResponseType(typeof(WeatherSummaryDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status502BadGateway)]
  public async Task<IActionResult> Get([FromQuery] string city, CancellationToken cancellationToken)
  {
    try
    {
      var summary = await _weatherService.GetCurrentWeatherAsync(city, cancellationToken);
      if (summary is null)
      {
        return NotFound(new { message = $"City '{city}' not found or weather unavailable" });
      }
      return Ok(summary);
    }
    catch (OperationCanceledException)
    {
      return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Request cancelled" });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Weather endpoint failed for {City}", city);
      return StatusCode(StatusCodes.Status502BadGateway, new { message = "Upstream weather service error" });
    }
  }
}