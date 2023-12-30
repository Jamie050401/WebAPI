namespace WebAPI.Configuration;

public enum DatabaseType
{
    InMemory,
    Sqlite
}

public static class Database
{
    public static DatabaseType GetDatabaseType(IConfiguration configuration)
    {
        var databaseType = configuration["Database:DatabaseType"];

        if (databaseType is null)
        {
            return DatabaseType.InMemory;
        }
        
        return databaseType.Trim().ToLower() switch
        {
            // ReSharper disable once StringLiteralTypo
            "inmemory" => DatabaseType.InMemory,
            "sqlite" => DatabaseType.Sqlite,
            _ => DatabaseType.Sqlite
        };
    }
}
