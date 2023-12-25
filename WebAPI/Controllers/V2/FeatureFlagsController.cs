using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers.V2;

[ApiController, ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/featureFlags/[action]")]
public class FeatureFlagsController : V1.FeatureFlagsController
{
    private readonly ApiContext _context;
    private readonly IConfiguration _configuration;

    public FeatureFlagsController(ApiContext context, IConfiguration configuration) : base(context, configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [HttpGet, ActionName("getSome")]
    public virtual JsonResult GetSome()
    {
        var result = _context.FeatureFlags.ToList();

        var some = result.OrderBy(_ => new Random().Next(0, result.Count - 1)).Take(result.Count / 2).ToList();
        
        return new JsonResult(Ok(some));
    }
}