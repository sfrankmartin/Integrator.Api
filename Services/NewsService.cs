using System.Net.Http.Json;
using Integrator.Api.Models.News;

namespace Integrator.Api.Services;

internal sealed class NewsService : INewsService
{
  private readonly IHttpClientFactory _httpClientFactory;

  public NewsService(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  public async Task<IReadOnlyList<NewsItemDto>> GetNewsAsync(string? query, CancellationToken cancellationToken)
  {
    var client = _httpClientFactory.CreateClient("hn-algolia");
    string path = string.IsNullOrWhiteSpace(query)
      ? "api/v1/search?tags=front_page"
      : $"api/v1/search?query={Uri.EscapeDataString(query)}&tags=story";

    var response = await client.GetFromJsonAsync<HnSearchResponse>(path, cancellationToken);
    var items = response?.Hits ?? new List<HnHit>();

    return items
      .Where(h => !string.IsNullOrWhiteSpace(h.Title))
      .Select(h => new NewsItemDto
      {
        Title = h.Title!,
        Url = h.Url,
        Author = h.Author,
        Points = h.Points ?? 0,
        CreatedAt = h.CreatedAt
      })
      .ToList();
  }
}