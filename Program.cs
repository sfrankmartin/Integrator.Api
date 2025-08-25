
namespace Integrator.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (System.IO.File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }
        });

        // HttpClient registrations
        builder.Services.AddHttpClient("open-meteo-forecast", client =>
        {
            client.BaseAddress = new Uri("https://api.open-meteo.com/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
        builder.Services.AddHttpClient("open-meteo-geocoding", client =>
        {
            client.BaseAddress = new Uri("https://geocoding-api.open-meteo.com/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });
        builder.Services.AddHttpClient("hn-algolia", client =>
        {
            client.BaseAddress = new Uri("https://hn.algolia.com/");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        builder.Services.AddScoped<Services.INewsService, Services.NewsService>();
        builder.Services.AddScoped<Services.IWeatherService, Services.WeatherService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
        }

        // PLEASE NOTE: Since this is a demonstration app, Swagger is exposed in production.
        // Normally this would only be available in development, enclosed in the `if` check above.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthorization();

        app.Run();
    }
}
