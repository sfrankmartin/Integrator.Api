using System.Text.Json.Serialization;

namespace Integrator.Api.Models.News;

internal sealed class HnSearchResponse
{
  [JsonPropertyName("hits")] public List<HnHit>? Hits { get; set; }
}

internal sealed class HnHit
{
  [JsonPropertyName("title")] public string? Title { get; set; }
  [JsonPropertyName("url")] public string? Url { get; set; }
  [JsonPropertyName("author")] public string? Author { get; set; }
  [JsonPropertyName("points")] public int? Points { get; set; }
  [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
}