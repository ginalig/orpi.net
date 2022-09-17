using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using opti.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Test Docker client Uri = "http://188.93.210.233:2375/"

app.MapPost("/install_docker", () =>
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
    var dockerClientUri = query.DockerClientUri;
    var containerId = query.ContainerId;
    DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
    var response = await client.Containers.InspectContainerAsync(containerId);
    bool isRunning = response.State.Running;
    if (isRunning)
    {
        return Results.BadRequest();
    }
    await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
    return Results.Ok();
});

app.MapGet("/stop", async ([FromBody]ContainerQuery query) =>
{
    var dockerClientUri = query.DockerClientUri;
    var containerId = query.ContainerId;
    DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
    await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
});

app.Run();