using Docker.DotNet;
using Docker.DotNet.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

DockerClient client;


app.MapGet("/create-postgres-container/{ip}", async (string ip) =>
{
    try
    {
        client = new DockerClientConfiguration(new Uri("http://" + ip + ":2375/")).CreateClient();

        if (!await ImagesContains("postgres:latest"))
        {
            await client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = "postgres",
                    Tag = "latest"
                },
                new AuthConfig
                {
                    Email = "test@example.com",
                    Username = "test",
                    Password = "password"
                },
                new Progress<JSONMessage>());
        }

        await client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "postgres",
            HostConfig = new HostConfig
            {
                DNS = new[] {"127.0.0.1"}
            }
        });
    }
    catch
    {
        return Results.StatusCode(500);
    }

    return Results.Ok("Container built with ip: " + ip);
});

async Task<bool> ImagesContains(string repoTag)
{
    IList<ImagesListResponse> images = await client.Images.ListImagesAsync(new ImagesListParameters());
    return images.Any(image => image.RepoTags.Contains(repoTag));
}

app.Run();