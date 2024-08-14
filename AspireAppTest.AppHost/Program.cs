var builder = DistributedApplication.CreateBuilder(args);

//var storage = builder.AddAzureStorage("StorageConnection");
//var blobs = storage.AddBlobs("BlobConnection");

var blobs = builder.AddConnectionString("blobs");
var keyVault = builder.AddConnectionString("secrets");

var apiService = builder.AddProject<Projects.AspireAppTest_ApiService>("apiservice")
    .WithReference(blobs)
    .WithReference(keyVault);

builder.Build().Run();
