namespace AspireFull.Web;

public class WeatherApiClient(HttpClient httpClient)
{
	public async Task<List<WeatherForecast>?> GetWeatherForecasts(CancellationToken cancellationToken = default) =>
		await httpClient.GetFromJsonAsync<List<WeatherForecast>>("/api/weatherforecast", cancellationToken);

	public async Task<List<WeatherStation>?> GetWeatherStations(CancellationToken cancellationToken = default) =>
		await httpClient.GetFromJsonAsync<List<WeatherStation>>("/api/weatherstations", cancellationToken);
}
