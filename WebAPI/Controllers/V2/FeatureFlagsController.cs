﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers.V2;

[ApiController, ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/featureFlags/[action]")]
public class FeatureFlagsController : ControllerBase
{
    private readonly V1.FeatureFlagsController _featureFlagsControllerV1;
    private readonly ApiContext _context;
    private readonly IConfiguration _configuration;
    private readonly Serilog.ILogger _logger;

    public FeatureFlagsController(ApiContext context, IConfiguration configuration, Serilog.ILogger logger)
    {
        _featureFlagsControllerV1 = new V1.FeatureFlagsController(context, configuration, logger);
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost, ActionName("createEdit")]
    public JsonResult CreateEdit(List<FeatureFlag> featureFlags)
    {
        foreach (var featureFlag in featureFlags)
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
        }
        
        _context.SaveChanges();

        return new JsonResult(Ok(featureFlags));
    }
    
    [HttpGet, ActionName("get")]
    public JsonResult Get(string feature)
    {
        return _featureFlagsControllerV1.Get(feature);
    }

    [HttpPost, ActionName("getSome")]
    public JsonResult GetSome(List<string> features)
    {
        var missingFeatureFlags = new List<string>();
        var featureFlags = new List<FeatureFlag>();
        foreach (var feature in features)
        {
            var featureFlagInDb = _context.FeatureFlags.Find(feature);
            
            if (featureFlagInDb is null)
            {
                missingFeatureFlags.Add(feature);
            }
            else
            {
                featureFlags.Add(featureFlagInDb);
            }
        }

        return missingFeatureFlags.Count > 0 ? new JsonResult(NotFound(featureFlags)) : new JsonResult(Ok(featureFlags));
    }
    
    [HttpGet, ActionName("getAll")]
    public JsonResult GetAll()
    {
        return _featureFlagsControllerV1.GetAll();
    }
    
    [HttpDelete, ActionName("delete")]
    public JsonResult Delete(List<string> features)
    {
        var missingFeatureFlags = new List<string>();
        foreach (var feature in features)
        {
            var featureFlagInDb = _context.FeatureFlags.Find(feature);

            if (featureFlagInDb is not null)
            {
                _context.FeatureFlags.Remove(featureFlagInDb);
            }
            else
            {
                missingFeatureFlags.Add(feature);
            }
        }
        
        _context.SaveChanges();

        return missingFeatureFlags.Count > 0 ? new JsonResult(NotFound(missingFeatureFlags)) : new JsonResult(NoContent());
    }
    
    [HttpPost, ActionName("toggle")]
    public JsonResult Toggle(string feature)
    {
        var featureFlagInDb = _context.FeatureFlags.Find(feature);

        if (featureFlagInDb is null)
        {
            return new JsonResult(NotFound());
        }

        featureFlagInDb.IsEnabled = !featureFlagInDb.IsEnabled;
        _context.SaveChanges();

        return new JsonResult(Ok(featureFlagInDb));
    }

    [HttpPost, ActionName("toggleSome")]
    public JsonResult ToggleSome(List<string> features)
    {
        var featureFlags = new List<FeatureFlag>();
        var missingFeatures = new List<string>();
        foreach (var feature in features)
        {
            var featureFlagInDb = _context.FeatureFlags.Find(feature);

            if (featureFlagInDb is not null)
            {
                featureFlagInDb.IsEnabled = !featureFlagInDb.IsEnabled;
                featureFlags.Add(featureFlagInDb);
            }
            else
            {
                missingFeatures.Add(feature);
            }
        }

        _context.SaveChanges();

        return missingFeatures.Count > 0 ? new JsonResult(NotFound(featureFlags)) : new JsonResult(Ok(featureFlags));
    }

    [HttpPost, ActionName("toggleAll")]
    public JsonResult ToggleAll()
    {
        var featureFlagsInDb = _context.FeatureFlags.ToList();

        foreach (var featureFlag in featureFlagsInDb)
        {
            featureFlag.IsEnabled = !featureFlag.IsEnabled;
        }

        _context.SaveChanges();

        return new JsonResult(Ok(featureFlagsInDb));
    }
}
