
var builder = Host.CreateApplicationBuilder(args);

// Add services to the container.

builder.Services.AddHostedService<Worker>();

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation();
    })
    .WithTracing(tracing =>
    {
        tracing.AddHttpClientInstrumentation((options) =>
        {
            options.EnrichWithHttpRequestMessage = (activity, httpRequestMessage) =>
            {
                activity.DisplayName = $"{httpRequestMessage.Method} {httpRequestMessage.RequestUri}";
            };
            options.EnrichWithHttpResponseMessage = (activity, httpResponseMessage) =>
            {
                activity.DisplayName = $"{(int)httpResponseMessage.StatusCode} {httpResponseMessage.RequestMessage?.Method} {httpResponseMessage.RequestMessage?.RequestUri}";
            };
            options.EnrichWithException = (activity, exception) =>
            {
                activity.SetTag("stackTrace", exception.StackTrace);
            };
        })
        .AddSource(Worker.ACTIVITY_SOURCE_NAME);
    });

builder.Services.AddOpenTelemetry().UseOtlpExporter();

string? apiServiceUrl = builder.Configuration.GetValue<string>("ApiServiceUrl");
ArgumentNullException.ThrowIfNullOrWhiteSpace(apiServiceUrl);
builder.Services.AddHttpClient("apiservice", client =>
{
    client.BaseAddress = new(apiServiceUrl);
});

//

var host = builder.Build();

//

host.Run();
