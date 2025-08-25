using Integrator.Api.Models.News;

namespace Integrator.Api.Services;

public interface INewsService
{
  Task<IReadOnlyList<NewsItemDto>> GetNewsAsync(string? query, CancellationToken cancellationToken);
}