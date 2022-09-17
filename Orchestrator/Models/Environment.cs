namespace Orchestrator.Models;

public class Environment
{
    public ulong ID { get; set; }
    public string IP { get; set; }
    public List<Service> Services { get; set; }
}