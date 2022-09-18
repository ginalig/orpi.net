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

    public static async Task<CreateContainerResponse> CreateContainer(Models.CreateContainerQuery query)
    {
        var dockerClientUri = query.DockerClientUri;
        DockerClient client = new DockerClientConfiguration(new Uri(dockerClientUri)).CreateClient();
        var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Name = query.Name,
            Platform = null,
            Hostname = null,
            Domainname = null,
            User = null,
            AttachStdin = false,
            AttachStdout = false,
            AttachStderr = false,
            ExposedPorts = query.ExposedPorts,
            Tty = query.Tty,
            OpenStdin = false,
            StdinOnce = false,
            Env = query.Env,
            Cmd = null,
            Healthcheck = null,
            ArgsEscaped = false,
            Image = query.Image,
            Volumes = query.Volumes,
            WorkingDir = null,
            Entrypoint = null,
            NetworkDisabled = false,
            MacAddress = null,
            OnBuild = null,
            Labels = null,
            StopSignal = null,
            StopTimeout = null,
            Shell = null,
            HostConfig = new HostConfig
            {
                Binds = null,
                ContainerIDFile = null,
                LogConfig = null,
                NetworkMode = null,
                PortBindings = query.PortBindings,
                RestartPolicy = null,
                AutoRemove = false,
                VolumeDriver = null,
                VolumesFrom = null,
                CapAdd = null,
                CapDrop = null,
                CgroupnsMode = null,
                DNS = null,
                DNSOptions = null,
                DNSSearch = null,
                ExtraHosts = null,
                GroupAdd = null,
                IpcMode = null,
                Cgroup = null,
                Links = null,
                OomScoreAdj = 0,
                PidMode = null,
                Privileged = false,
                PublishAllPorts = false,
                ReadonlyRootfs = false,
                SecurityOpt = null,
                StorageOpt = null,
                Tmpfs = null,
                UTSMode = null,
                UsernsMode = null,
                ShmSize = 0,
                Sysctls = null,
                Runtime = null,
                ConsoleSize = new ulong[]
                {
                },
                Isolation = null,
                CPUShares = 0,
                Memory = 0,
                NanoCPUs = 0,
                CgroupParent = null,
                BlkioWeight = 0,
                BlkioWeightDevice = null,
                BlkioDeviceReadBps = null,
                BlkioDeviceWriteBps = null,
                BlkioDeviceReadIOps = null,
                BlkioDeviceWriteIOps = null,
                CPUPeriod = 0,
                CPUQuota = 0,
                CPURealtimePeriod = 0,
                CPURealtimeRuntime = 0,
                CpusetCpus = null,
                CpusetMems = null,
                Devices = null,
                DeviceCgroupRules = null,
                DeviceRequests = null,
                KernelMemory = 0,
                KernelMemoryTCP = 0,
                MemoryReservation = 0,
                MemorySwap = 0,
                MemorySwappiness = null,
                OomKillDisable = null,
                PidsLimit = null,
                Ulimits = null,
                CPUCount = 0,
                CPUPercent = 0,
                IOMaximumIOps = 0,
                IOMaximumBandwidth = 0,
                Mounts = null,
                MaskedPaths = null,
                ReadonlyPaths = null,
                Init = null
            },
            NetworkingConfig = null
        });
        return response;
    }
}