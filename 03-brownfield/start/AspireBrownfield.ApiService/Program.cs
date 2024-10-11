using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProblemDetails();

// local debugging: "localhost:6379"
string? redisConnStr = builder.Configuration.GetConnectionString("Redis");
ArgumentNullException.ThrowIfNullOrWhiteSpace(redisConnStr);
builder.Services.AddStackExchangeRedisOutputCache(options => {
	options.Configuration = redisConnStr;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseExceptionHandler();
app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

List<string> summaries = [ "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" ];

app.MapGet("/api/weatherforecast", [OutputCache(Duration = 3/*sec*/)] () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Count)]
        ))
        .ToArray();
    return forecast;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
