var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Fhi_Kompetanse_AI_SK_ApiService>("apiservice");

builder.AddProject<Projects.Fhi_Kompetanse_AI_SK_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
