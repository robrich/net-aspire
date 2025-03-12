var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithImageTag("alpine");

var apiService = builder.AddProject<Projects.AspireGreenfield_ApiService>("apiservice");

builder.AddProject<Projects.AspireGreenfield_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
