using Docker.DotNet;
using Docker.DotNet.Models;

namespace orpi.Utils;

public class Container
{
    public static async Task<bool> TryContainerStart(Models.ContainerQuery query)
    {
        var dockerClientUri = query.DockerClientUri;
        var containerId = query.ContainerId;
        DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
        var response = await client.Containers.InspectContainerAsync(containerId);
        bool isSuccess = !response.State.Running;
        if (!isSuccess)
        {
            return false;
        }
        await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
        return true;
    }

    public static async Task ContainerStop(Models.ContainerQuery query)
    {
        var dockerClientUri = query.DockerClientUri;
        var containerId = query.ContainerId;
        DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
        await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
    }
}