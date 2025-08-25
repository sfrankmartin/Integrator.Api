# Integrator.Api

A small, .NET 8 Web API demonstrating how to consume external APIs and expose clean, typed endpoints with some light transformation/aggregation. It’s designed as a portfolio/example project.

## What it does

* Fetches current weather for a city from an external weather API and returns a trimmed/typed summary.
* Fetches news from an external news API, returning a normalized list of stories.
* Provides a Dashboard endpoint that aggregates weather + news into a single response.

## Tech & Architecture
* Runtime: .NET 8 / ASP.NET Core Web API
* Docs/UI: Swagger (Swashbuckle) enabled in all environments
* HTTP: HttpClient via DI, async/await throughout, CancellationToken respected
* Structure:
  * Controllers (/Controllers): Thin HTTP endpoints for Weather, News, Dashboard
  * Services (/Services): WeatherService, NewsService encapsulate external calls & mapping
  * Models (/Models): DTOs for inbound/outbound payloads (e.g., OpenMeteoDtos, HnDtos, WeatherSummaryDto, NewsItemDto, DashboardDto)
  * Program.cs: Minimal hosting, Swagger, DI registrations

## Endpoints

All endpoints are under the base path: `/api`.

`GET /api/weather`

Query params:
* `city` (required) — e.g. Melbourne

Returns: `WeatherSummaryDto`
* A clean summary of current conditions derived from the upstream weather API.

Error behavior (examples):
* 400 Bad Request when city is missing/blank
* 404 Not Found if the city isn’t recognized or upstream has no data
* 504 Gateway Timeout if the request was cancelled
* 502 Bad Gateway for upstream failures (logged)

⸻

`GET /api/news`

Query params:
* `query` (optional) — filter/search term (e.g. dotnet)

Returns: `IReadOnlyList<NewsItemDto>`
* Normalized news items from the upstream news API.

Error behavior (examples):
* 504 Gateway Timeout on cancel
* 502 Bad Gateway for upstream failures (logged)

⸻

`GET /api/dashboard`

Query params:
* `city` (required) — passed to weather
* `query` (optional) — passed to news

Returns: `DashboardDto` with:
* weather: `WeatherSummaryDto`
* news: `IReadOnlyList<NewsItemDto>`

Error behavior matches the individual services, with consistent 4xx/5xx responses and logging.

## External APIs
* Weather: Open‑Meteo style responses mapped to `OpenMeteoDtos` and collapsed into `WeatherSummaryDto`.
* News: Hacker News/Algolia style responses mapped to `HnDtos` and normalized into `NewsItemDto`.

## Quickstart

1) Requirements: .NET 8 SDK

2) Restore & run:
    ```
    cd Integrator.Api
    dotnet restore
    dotnet run
    ```

    The app will start (by default) on something like http://localhost:5129.

3) Explore Swagger

    Open:

    http://localhost:5129/swagger

    Use the Schemas and Try it out buttons to explore the API contract and make calls.

    Note: For demonstration purposes, Swagger is enabled even in production. In a real app I’d typically guard it behind `if (app.Environment.IsDevelopment())`.

## Configuration

The demo does not require API keys. If you adapt this to a paid provider, add your keys via user‑secrets or environment variables and bind a typed options class.

## Development notes
* DTOs over raw JSON: Upstream payloads are deserialized into dedicated *Dtos and then shaped into outward‑facing DTOs, so the public contract is stable even if providers change.
* Cancellation support: Controllers accept a CancellationToken and translate OperationCanceledException into a 504.
* Guard clauses & validation: Required query params are checked early and return 400 with a helpful message when missing.
* Logging: Failures and unexpected conditions are logged with contextual data (e.g., city name) to help debugging.
* Testability: Controllers depend on interfaces (`IWeatherService`, `INewsService`) allowing unit testing with mocks/fakes.

## Status & license

This is a demonstration repository intended for portfolio use, no explicit license is included.