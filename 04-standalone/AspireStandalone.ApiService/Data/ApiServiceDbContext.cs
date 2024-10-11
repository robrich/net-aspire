namespace AspireFull.ApiService.Data;

public class ApiServiceDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<WeatherStation> WeatherStations => Set<WeatherStation>();
}
