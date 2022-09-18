using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;
using PostgresModule.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

DockerClient client;


app.MapPost("/configure", async ([FromBody]ConfigureQuery query) =>
{
    try
    {
        client = new DockerClientConfiguration(new Uri(query.DockerClientUri)).CreateClient();
        
        if (!await ImagesContains("postgres:latest"))
        {
            await client.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = "postgres",
                    Tag = "latest"
                },
                new AuthConfig(),
                new Progress<JSONMessage>());
        }

        await client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "postgres",
            HostConfig = new HostConfig()
            {
                PortBindings = query.PortBindings
            }
        });
    }
    catch
    {
        return Results.StatusCode(500);
    }

    return Results.Ok();
});

async Task<bool> ImagesContains(string repoTag)
{
    IList<ImagesListResponse> images = await client.Images.ListImagesAsync(new ImagesListParameters());
    return images.Any(image => image.RepoTags.Contains(repoTag));
}

app.Run();