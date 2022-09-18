namespace Orchestrator.Models;

public class ServiceRequest
{
    public string DockerClientUri { get; set; }
    public string CommandData { get; set; }
    public ServiceType Type { get; set; }
}