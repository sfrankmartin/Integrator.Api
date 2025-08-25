using Integrator.Api.Models.News;
using Integrator.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Integrator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NewsController : ControllerBase
{
  private readonly INewsService _newsService;
  private readonly ILogger<NewsController> _logger;
  public NewsController(INewsService newsService, ILogger<NewsController> logger)
  {
    _newsService = newsService;
    _logger = logger;
  }

  /// <summary>
  /// Gets latest HackerNews front page story, or query results
  /// </summary>
  /// <param name="query">Optional search query</param>
  /// <param name="cancellationToken">Cancellation token</param>
  [HttpGet]
  [ProducesResponseType(typeof(IReadOnlyList<NewsItemDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status502BadGateway)]
  public async Task<IActionResult> Get([FromQuery(Name = "q")] string? query, CancellationToken cancellationToken)
  {
    try
    {
      var items = await _newsService.GetNewsAsync(query, cancellationToken);
      return Ok(items);
    }
    catch (OperationCanceledException)
    {
      return StatusCode(StatusCodes.Status504GatewayTimeout, new { message = "Request cancelled" });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "News endpoint failed for query {Query}", query);
      return StatusCode(StatusCodes.Status502BadGateway, new { message = "Upstream news service error" });
    }

  }
}