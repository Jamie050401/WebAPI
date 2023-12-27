#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
#pragma warning disable CS8524 // The switch expression does not handle some values of its input type (it is not exhaustive) involving an unnamed enum value.

namespace WebAPI.Configuration;

public enum EnvironmentType
{
    Local,
    Development,
    SystemTest,
    Runway,
    Staging,
    Taxiway,
    Production
}

public class Environment
{
    private EnvironmentType Type { get; set; }

    private Environment(EnvironmentType environmentType)
    {
        Type = environmentType;
    }
    
    public static Environment GetEnvironmentType(IConfiguration configuration, string? environmentName = null)
    {
        var environment = environmentName ?? configuration["Environment:Type"];

        if (environment is null)
        {
            return new Environment(EnvironmentType.Production);
        }

        return environment.Trim().ToLower() switch
        {
            "local" => new Environment(EnvironmentType.Local),
            "development" => new Environment(EnvironmentType.Development),
            // ReSharper disable once StringLiteralTypo
            "systemtest" => new Environment(EnvironmentType.SystemTest),
            "runway" => new Environment(EnvironmentType.Runway),
            "staging" => new Environment(EnvironmentType.Staging),
            "taxiway" => new Environment(EnvironmentType.Taxiway),
            "production" => new Environment(EnvironmentType.Production)
        };
    }

    public string AsString()
    {
        return Type switch
        {
            EnvironmentType.Local => "Local",
            EnvironmentType.Development => "Development",
            EnvironmentType.SystemTest => "SystemTest",
            EnvironmentType.Runway => "Runway",
            EnvironmentType.Staging => "Staging",
            EnvironmentType.Taxiway => "Taxiway",
            EnvironmentType.Production => "Production"
        };
    }

    public static bool IsDebugEnabled(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Local or EnvironmentType.Development;
    }

    public static bool IsLocal(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Local;
    }
    
    public static bool IsDevelopment(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Development;
    }
    
    public static bool IsSystemTest(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.SystemTest;
    }
    
    public static bool IsRunway(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Runway;
    }
    
    public static bool IsStaging(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Staging;
    }
    
    public static bool IsTaxiway(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Taxiway;
    }
    
    public static bool IsProduction(IConfiguration configuration, EnvironmentType? environmentType = null)
    {
        var environment = environmentType ?? GetEnvironmentType(configuration).Type;

        return environment is EnvironmentType.Production;
    }
}
