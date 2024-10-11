
using AspireStandalone.ApiService;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddProblemDetails();

string? redisConnStr = builder.Configuration.GetConnectionString("Redis");
ArgumentException.ThrowIfNullOrWhiteSpace(redisConnStr);
var redisConn = ConnectionMultiplexer.Connect(redisConnStr);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisConn);
builder.Services.AddStackExchangeRedisOutputCache(options => {
    options.ConnectionMultiplexerFactory = async () => await Task.FromResult(redisConn);
});

string? postgresConnStr = builder.Configuration.GetConnectionString("Postgres");
ArgumentException.ThrowIfNullOrWhiteSpace(postgresConnStr);
builder.Services.AddDbContext<ApiServiceDbContext>(options => options.UseNpgsql(postgresConnStr));

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation();
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddRedisInstrumentation(redisConn);
    });

builder.Services.AddOpenTelemetry().UseOtlpExporter();

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

app.MapAPIs();

app.Run();
