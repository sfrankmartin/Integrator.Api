namespace Integrator.Api.Models.News;

public sealed class NewsItemDto
{
  public required string Title { get; init; }
  public string? Url { get; init; }
  public string? Author { get; init; }
  public int Points { get; init; }
  public DateTime CreatedAt { get; init; }
}