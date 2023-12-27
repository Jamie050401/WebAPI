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
    public JsonResult CreateEdit(FeatureFlag featureFlag)
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
    public JsonResult Get(string feature)
    {
        var featureFlagInDb = _context.FeatureFlags.Find(feature);

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (featureFlagInDb is null)
        {
            return new JsonResult(NotFound());
        }

        return new JsonResult(Ok(featureFlagInDb));
    }

    [HttpGet, ActionName("getAll")]
    public JsonResult GetAll()
    {
        var featureFlagsInDb = _context.FeatureFlags.ToList();

        return new JsonResult(Ok(featureFlagsInDb));
    }

    [HttpDelete, ActionName("delete")]
    public JsonResult Delete(string feature)
    {
        var featureFlagInDb = _context.FeatureFlags.Find(feature);

        if (featureFlagInDb is null)
        {
            return new JsonResult(NotFound());
        }

        _context.FeatureFlags.Remove(featureFlagInDb);
        _context.SaveChanges();

        return new JsonResult(NoContent());
    }
}
