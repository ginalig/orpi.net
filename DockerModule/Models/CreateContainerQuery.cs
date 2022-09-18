using Docker.DotNet.Models;

namespace orpi.Models;

public class CreateContainerQuery
{
    public string DockerClientUri { get; set; }
    public string Name { get; set; }
    public bool Tty { get; set; }
    public List<string> Env { get; set; }
    public string Image { get; set; }
    public Dictionary<string, EmptyStruct> Volumes { get; set; }
    public Dictionary<string, EmptyStruct> ExposedPorts { get; set; }
    public Dictionary<string, IList<PortBinding>> PortBindings { get; set; }
}