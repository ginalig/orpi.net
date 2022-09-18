using System.Net;
using System.Text;
using System.Text.Json;
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
    private static ulong id = 0;
    
    static DbManager db = new DbManager();
    
    
    [HttpPost]
    [Route("post-form")]
    public async Task<IActionResult> ProcessUserForm([FromBody] EnvCreateRequest form)
    {
        db.Insert(new Environment
        {
            ID = id++,
            IP = form.IP,
            Services = form.Services
        });

        foreach (var formService in form.Services)
        {
            var res = await http.PostAsync(ResolveModule(formService.Type) + "/configure", new StringContent(""));
            if (res?.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode(500);
            }
        }
        
        return Ok();
    }

    private string ResolveModule(ServiceType type)
    {
        switch (type)
        {
            case ServiceType.Docker:
                return "http://localhost:3000";
                break;
            case ServiceType.Postgres:
                return "http://localhost:3001";
                break;
            case ServiceType.Nginx:
                return "http://localhost:3002";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [HttpPost]
    [Route("request-service")]
    public async Task<IActionResult> RequestService([FromBody] ServiceRequest request)
    {
        var res = await http.PostAsync(ResolveModule(request.Type) + "/request", 
            new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
                ));
        if (res?.StatusCode != HttpStatusCode.OK)
        {
            return StatusCode(500);
        }
        return Ok();
    }
}