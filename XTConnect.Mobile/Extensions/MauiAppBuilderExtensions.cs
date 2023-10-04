using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;
using XTConnect.Core.Interfaces;
using XTConnect.Core.Services;

namespace XTConnect.Mobile.Extensions;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureAppSettings(this MauiAppBuilder builder, string resourcePath)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream(resourcePath);

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);

        return builder;
    }

    public static MauiAppBuilder SetupSerilog(this MauiAppBuilder builder)
    {
        var assemblyInfo = Assembly.GetExecutingAssembly().GetName();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Destructure.AsScalar<JObject>()
            .Destructure.AsScalar<JArray>()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Debug()
            .Enrich.WithProperty("ApplicationName", assemblyInfo.Name)
            .Enrich.WithProperty("ApplicationVersion", assemblyInfo.Version)
            .CreateLogger();

        return builder;
    }

    public static MauiAppBuilder UseVLinkDataService(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IXTConnectDataService, XTConnectDataService>();
        return builder;
    }
}
