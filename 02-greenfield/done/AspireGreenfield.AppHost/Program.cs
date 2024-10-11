var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache")
    .WithImageTag("alpine");

var apiService = builder.AddProject<Projects.AspireGreenfield_ApiService>("apiservice");

builder.AddProject<Projects.AspireGreenfield_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
