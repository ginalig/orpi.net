using System.Net;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Models;
using Orchestrator.Utils;
using Environment = Orchestrator.Models.Environment;

namespace Orchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrchestratorController : Controller
{
    private static HttpClient http = new HttpClient();
    private static List<Environment> db = new List<Environment>();
    private static ulong id = 0;
    
    //static DbManager db = new DbManager();
    
    
    [HttpPost]
    [Route("post-form")]
    public async Task<IActionResult> ProcessUserForm([FromBody] EnvCreateRequest form)
    {
        db.Add(new Environment
        {
            ID = id++,
            IP = form.IP,
            Services = form.Services
        });

        foreach (var formService in form.Services)
        {
            var res = await ResolveModule(formService);
            if (res?.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode(500);
            }
        }
        
        return Ok();
    }

    private async Task<HttpResponseMessage?> ResolveModule(Service service)
    {
        switch (service.Type)
        {
            case ServiceType.Docker:
                return await http.PostAsync("http://localhost:3000/install_docker", new StringContent(""));
                break;
            case ServiceType.Postgres:
                return await http.PostAsync("http://localhost:3001/configure", new StringContent(""));
                break;
            case ServiceType.Nginx:
                return await http.PostAsync("http://localhost:3002/configure", new StringContent(""));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    
}