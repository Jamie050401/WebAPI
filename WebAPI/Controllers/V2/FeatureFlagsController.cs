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

    public FeatureFlagsController(ApiContext context) : base(context)
    {
        _context = context;
    }
    
    [HttpGet, ActionName("getSome")]
    public virtual JsonResult GetSome()
    {
        var result = _context.FeatureFlags.ToList();

        var some = result.OrderBy(_ => new Random().Next(0, result.Count - 1)).Take(result.Count / 2).ToList();
        
        return new JsonResult(Ok(some));
    }
}