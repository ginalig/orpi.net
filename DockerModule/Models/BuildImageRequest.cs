namespace orpi.Models;

public class BuildImageRequest
{
    public string DockerClientUri { get; set; }
    public string Name { get; set; }
    public string Filename { get; set; }
    public string CodeBaseUri { get; set; }
}