namespace AspireStandalone.LoadTest;

public class Worker(IHttpClientFactory httpClientFactory, ILogger<Worker> logger) : BackgroundService
{
    public const string ACTIVITY_SOURCE_NAME = "worker";
    private static readonly ActivitySource MyActivitySource = new(ACTIVITY_SOURCE_NAME);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var httpClient = httpClientFactory.CreateClient("apiservice");
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var activity = MyActivitySource.StartActivity("LoadTest"))
            {
                await GetForecasts(httpClient, stoppingToken);
                await GetStations(httpClient, stoppingToken);
            }
            await Task.Delay(2000, stoppingToken);
        }
    }

    private async Task GetForecasts(HttpClient httpClient, CancellationToken cancellationToken)
    {
        using (var activity = MyActivitySource.StartActivity("WeatherForecasts"))
        {
            logger.LogInformation($"Getting Forecasts at: {DateTimeOffset.Now.ToUniversalTime()}");
            try
            {
                var res = await httpClient.GetFromJsonAsync<WeatherForecast[]>("/api/weatherforecast", cancellationToken);
                logger.LogInformation($"Result: {res?.Length} forecasts");
                activity?.SetTag("Forecasts", res?.Length);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in WeatherForecasts");
            }
        }
    }

    private async Task GetStations(HttpClient httpClient, CancellationToken cancellationToken)
    {
        using (var activity = MyActivitySource.StartActivity("WeatherStations"))
        {
            logger.LogInformation($"Getting Weather Stations at: {DateTimeOffset.Now.ToUniversalTime()}");
            try
            {
                var res = await httpClient.GetFromJsonAsync<WeatherStation[]>("/api/weatherstations", cancellationToken);
                logger.LogInformation($"Result: {res?.Length} weather stations");
                activity?.SetTag("Stations", res?.Length);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in WeatherStations");
            }
        }
    }

}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record WeatherStation(string Id, string Name, string Timezone, string Country, double Latitude, double Longitude, int Elevation);
