using Docker.DotNet.Models;

namespace PostgresModule.Models;

public class ConfigureQuery
{
    public string DockerClientUri { get; set; }
    
    public Dictionary<string, IList<PortBinding>> PortBindings { get; set; }
}