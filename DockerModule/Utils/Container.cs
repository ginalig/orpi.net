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

    public static async Task TryBuildImageFromDockerFile(Models.BuildImageRequest query)
    {
        var dockerClientUri = query.DockerClientUri;
        DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
        await client.Images.BuildImageFromDockerfileAsync(
            new ImageBuildParameters
            {
                Tags = new List<string>(){query.Name},
                SuppressOutput = null,
                RemoteContext = null,
                NoCache = null,
                Remove = false,
                ForceRemove = null,
                PullParent = null,
                Pull = null,
                Isolation = null,
                CPUSetCPUs = null,
                CPUSetMems = null,
                CPUShares = null,
                CPUQuota = null,
                CPUPeriod = null,
                Memory = null,
                MemorySwap = null,
                CgroupParent = null,
                NetworkMode = null,
                ShmSize = null,
                Dockerfile = "./test-app/Dockerfile",
                Ulimits = null,
                BuildArgs = null,
                Labels = null,
                Squash = null,
                CacheFrom = null,
                SecurityOpt = null,
                ExtraHosts = null,
                Target = null,
                SessionID = null,
                Platform = null,
                Outputs = null,
                AuthConfigs = null
            },
            new FileStream(query.Filename, FileMode.Open),
            new List<AuthConfig>() {},
            new Dictionary<string, string>() {},
            new Progress<JSONMessage>() {},
            new CancellationToken());
    }
}