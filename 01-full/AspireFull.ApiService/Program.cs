
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.AddNpgsqlDbContext<ApiServiceDbContext>("postgresdb");

builder.AddRedisOutputCache("cache");

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

app.MapDefaultEndpoints();

app.Run();
