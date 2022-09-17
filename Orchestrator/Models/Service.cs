namespace Orchestrator.Models;

public class Service
{
    public ulong ID { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
    public ServiceType Type { get; set; }
    public string Ports { get; set; }
}