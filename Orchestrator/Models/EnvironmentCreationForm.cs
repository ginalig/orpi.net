namespace Orchestrator.Models;

public class EnvCreateRequest
{
    public string Name { get; set; }
    public string IP { get; set; }
    public List<Service> Services { get; set; }
}