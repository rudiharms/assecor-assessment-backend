using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Assecor.Api.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _testCsvPath;

    public CustomWebApplicationFactory()
    {
        var testId = Guid.NewGuid().ToString("N")[..8];
        _testCsvPath = Path.Combine(Path.GetTempPath(), $"test-data-{testId}.csv");

        var sourceFile = Path.Combine(Directory.GetCurrentDirectory(), "test-data.csv");

        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, _testCsvPath, true);
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
            {
                var configData = new Dictionary<string, string>
                {
                    ["SqlOptions:UseSql"] = "false",
                    ["CsvOptions:FilePath"] = _testCsvPath,
                    ["CsvOptions:Delimiter"] = ",",
                    ["Serilog:MinimumLevel:Default"] = "Warning"
                };

                config.AddInMemoryCollection(configData!);
            }
        );

        builder.UseEnvironment("Test");

        builder.ConfigureServices(static services =>
            {
                services.Configure<HostOptions>(static opts => opts.ShutdownTimeout = TimeSpan.FromSeconds(1));

                services.Configure<JsonOptions>(static options =>
                    {
                        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    }
                );
            }
        );
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && File.Exists(_testCsvPath))
        {
            try
            {
                File.Delete(_testCsvPath);
            }
            catch
            {
                // Ignore errors
            }
        }

        base.Dispose(disposing);
    }
}
