using AspireBrownfield.Web;
using AspireBrownfield.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// local debugging: "localhost:6379"
string? redisConnStr = builder.Configuration.GetConnectionString("Redis");
ArgumentNullException.ThrowIfNullOrWhiteSpace(redisConnStr);
builder.Services.AddStackExchangeRedisOutputCache(options => {
	options.Configuration = redisConnStr;
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        string? apiUrl = builder.Configuration.GetValue<string>("AppSettings:WeatherApiUrl");
        ArgumentNullException.ThrowIfNullOrWhiteSpace(apiUrl);
        client.BaseAddress = new Uri(apiUrl);
    });

//

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
