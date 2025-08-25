using Integrator.Api.Models.Dashboard;
using Integrator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Integrator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
  private readonly IWeatherService _weatherService;
  private readonly INewsService _newsService;
  private readonly ILogger<DashboardController> _logger;

  public DashboardController(IWeatherService weatherService, INewsService newsService, ILogger<DashboardController> logger)
  {
    _weatherService = weatherService;
    _newsService = newsService;
    _logger = logger;
  }

  [HttpGet]
  [ProducesResponseType(typeof(DashboardDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status502BadGateway)]
  public async Task<IActionResult> Get([FromQuery] string city = "Melbourne", [FromQuery(Name = "q")] string? newsQuery = null, CancellationToken cancellationToken = default)
  {
    try
    {
      var weatherTask = _weatherService.GetCurrentWeatherAsync(city, cancellationToken);
      var newsTask = _newsService.GetNewsAsync(newsQuery, cancellationToken);

      await Task.WhenAll(weatherTask, newsTask);

      var dto = new DashboardDto
      {
        Weather = weatherTask.Result,
        TopNews = newsTask.Result.Take(5).ToList()
      };

      return Ok(dto);
    }
    catch (OperationCanceledException)
    {
      return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Request cancelled" });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Dashboard endpoint failed for city {City}", city);
      return StatusCode(StatusCodes.Status502BadGateway, new { message = "Upstream service error" });
    }
  }
}