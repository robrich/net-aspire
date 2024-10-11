var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("postgresql-password", secret: true);
var sqlUsername = builder.AddParameter("postgresql-username", secret: true);
var dbName = "weatherdb";
var postgres = builder.AddPostgres("postgres", userName: sqlUsername, password: sqlPassword)
	.WithImageTag("alpine")
	.WithEnvironment("POSTGRES_DB", dbName)
	.WithBindMount("../postgres-init", "/docker-entrypoint-initdb.d")
	.WithDataBindMount("../pg-data")
	.WithPgWeb(c => c.WithImageTag("latest"));

var postgresdb = postgres.AddDatabase("postgresdb", dbName);

var cache = builder.AddRedis("cache")
	.WithImageTag("alpine");

var apiService = builder.AddProject<Projects.AspireFull_ApiService>("apiservice")
	.WithReference(cache)
	.WithReference(postgresdb)
	.WithExternalHttpEndpoints();

builder.AddProject<Projects.AspireFull_Web>("webfrontend")
	.WithReference(cache)
	.WithReference(apiService)
	.WithExternalHttpEndpoints();

var vue = builder.AddNpmApp("vue", "../vue-app", "dev")
    .WithReference(apiService)
    .WithHttpEndpoint(env: "PORT" /*, isProxied: true*/)
	.WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
