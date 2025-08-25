namespace Integrator.Api.Models.News;

/// <summary>
/// Simplified news item returned by our API
/// </summary>
public sealed class NewsItemDto
{
  public required string Title { get; init; }
  public string? Url { get; init; }
  public string? Author { get; init; }
  public int Points { get; init; }
  public DateTime CreatedAt { get; init; }
}