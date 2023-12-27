using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using WebAPI.Configuration;
using Environment = WebAPI.Configuration.Environment;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    // ReSharper disable once StringLiteralTypo
    .AddJsonFile("appsettings.json")
    .Build());

builder.Environment.EnvironmentName = Environment.GetEnvironmentType(builder.Configuration).AsString();

// TODO - Implement support for additional (environment-specific) app settings files

// TODO - Implement serilog as logger of choice

builder.Services.AddDbContext<ApiContext>(opt =>
{
    var databaseType = Database.GetDatabaseType(builder.Configuration);

    // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
    switch (databaseType)
    {
        case DatabaseType.InMemory:
            opt.UseInMemoryDatabase("Database");
            break;
        case DatabaseType.Sqlite:
            opt.UseSqlite(builder.Configuration["Database:ConnectionStrings:Sqlite"]);
            break;
    }
});

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
}).AddApiExplorer(opt =>
{
    // ReSharper disable once StringLiteralTypo
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (Environment.IsDebugEnabled(app.Configuration))
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            opt.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
