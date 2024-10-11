namespace AspireStandalone.ApiService;

public static class APIs
{
    public static WebApplication MapAPIs(this WebApplication app)
    {
        app.MapGet("/api/weatherforecast", GetWeatherForecasts);

        app.MapGet("/api/weatherstations", GetWeatherStations);

        return app;
    }

    private static string[] summaries = [ "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" ];

    [OutputCache(Duration = 3/*sec*/)]
    private static List<WeatherForecast> GetWeatherForecasts() =>
        Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToList();

    private static List<WeatherStation> GetWeatherStations(ApiServiceDbContext db) =>
        db.WeatherStations.ToList();

}
