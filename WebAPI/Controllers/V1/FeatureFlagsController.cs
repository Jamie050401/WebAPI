using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers.V1;

[ApiController, ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/featureFlags/[action]")]
public class FeatureFlagsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IConfiguration _configuration;

    public FeatureFlagsController(ApiContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost, ActionName("createEdit")]
    public virtual JsonResult CreateEdit(FeatureFlag featureFlag)
    {
        var featureFlagInDb = _context.FeatureFlags.Find(featureFlag.Feature);

        if (featureFlagInDb is null)
        {
            _context.FeatureFlags.Add(featureFlag);
        }
        else
        {
            featureFlagInDb.IsEnabled = featureFlag.IsEnabled;
        }

        _context.SaveChanges();

        return new JsonResult(Ok(featureFlag));
    }

    [HttpGet, ActionName("get")]
    public virtual JsonResult Get(string feature)
    {
        var result = _context.FeatureFlags.Find(feature);

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (result is null)
        {
            return new JsonResult(NotFound());
        }

        return new JsonResult(Ok(result));
    }

    [HttpGet, ActionName("getAll")]
    public virtual JsonResult GetAll()
    {
        var result = _context.FeatureFlags.ToList();

        return new JsonResult(Ok(result));
    }

    [HttpDelete, ActionName("delete")]
    public virtual JsonResult Delete(string feature)
    {
        var result = _context.FeatureFlags.Find(feature);

        if (result is null)
        {
            return new JsonResult(NotFound());
        }

        _context.FeatureFlags.Remove(result);
        _context.SaveChanges();

        return new JsonResult(NoContent());
    }
}
