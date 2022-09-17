using Docker.DotNet;
using Docker.DotNet.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DockerClient client = new DockerClientConfiguration(
        new Uri("http://188.93.210.233:2375/"))
    .CreateClient();

app.MapGet("/create_network", async () =>
{
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
        Driver = "bridge",
        Scope = "local",
        EnableIPv6 = false,
        Ingress = false,
        Internal = false,
        IPAM = new IPAM()
        {
            Config = new List<IPAMConfig>() {new IPAMConfig() {Subnet = "172.20.0.0/16", Gateway = "172.20.0.1"}},
            Driver = "default",
            Options = new Dictionary<string, string>()
        },
        Labels = new Dictionary<string, string>(),
        Name = "test-net3",

    });
    return response.ID;
});

app.MapGet("/connect", async () =>
{
    await client.Networks.ConnectNetworkAsync("test-net3", new NetworkConnectParameters()
    {
        Container = "test",
        EndpointConfig = new EndpointSettings
        {
            IPAMConfig = new EndpointIPAMConfig()
            {
                IPv4Address = "172.20.0.2"
            }
        }
    });
    return 200;
});
app.Run();