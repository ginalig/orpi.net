using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using orpi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Test Docker client Uri = "http://188.93.210.233:2375/"

app.MapPost("/configure", () =>
{
    Results.Ok();
});

app.MapGet("/create_network", async ([FromBody]NetworkQuery query) =>
{
    var dockerClientUri = query.DockerClientUri;
    DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
    var response = await client.Networks.CreateNetworkAsync(new NetworksCreateParameters
    {
        Attachable = false,
        CheckDuplicate = false,
        ConfigFrom = new ConfigReference
        {
            Network = ""
        },
        Options = new Dictionary<string, string>(),
        ConfigOnly = false,
        Driver = query.Driver, //
        Scope = query.Scope, //
        EnableIPv6 = false,
        Ingress = false,
        Internal = false,
        IPAM = new IPAM()
        {
            Config = new List<IPAMConfig>(),
            Driver = "default",
            Options = new Dictionary<string, string>()
        },
        Labels = new Dictionary<string, string>(),
        Name = query.Name, // 

    });
    return response.ID;
});

app.MapGet("/connect", async ([FromBody]ConnectQuery query) =>
{
    var dockerClientUri = query.DockerClientUri;
    DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
    await client.Networks.ConnectNetworkAsync(query.NetworkName, new NetworkConnectParameters()
    {
        Container = query.ContainerName,
        EndpointConfig = new EndpointSettings
        {
        }
    });
    return Results.Ok();
});

app.MapGet("/start", async ([FromBody]ContainerQuery query) =>
{
    var isSuccess = await orpi.Utils.Container.TryContainerStart(query);
    if (!isSuccess)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.MapGet("/stop", async ([FromBody]ContainerQuery query) =>
    await orpi.Utils.Container.ContainerStop(query));


app.MapPost("/request", async ([FromBody]RequestQuery query) =>
{
    var args = query.CommandData.Split(';');
    switch (args[0])
    {
        case "start":
            var isSuccess = await orpi.Utils.Container.TryContainerStart(new ContainerQuery
            {
                ContainerId = args[1],
                DockerClientUri = query.DockerClientUri
            });
            if (!isSuccess)
            {
                return Results.BadRequest();
            }

            return Results.Ok();

        case "stop" :
            await orpi.Utils.Container.ContainerStop(new ContainerQuery
            {
                ContainerId = args[1],
                DockerClientUri = query.DockerClientUri
            });
            return Results.Ok();
    }

    return Results.BadRequest();
});

app.MapPost("/build_from_file", async ([FromBody]BuildImageRequest query) =>
{
    await orpi.Utils.Container.TryBuildImageFromDockerFile(query);
    return Results.Ok();
});

app.MapPost("/create_container", async ([FromBody] CreateContainerQuery query) =>
{
    return await orpi.Utils.Container.CreateContainer(query);
});

app.Run();