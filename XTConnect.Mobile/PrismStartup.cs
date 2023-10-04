
using XTConnect.Mobile.ViewModels;
using XTConnect.Mobile.Views;
using XTConnect.Module.AppUI;
using XTConnect.Module.Login;

namespace XTConnect.Mobile;

internal static class PrismStartup
{
    public static void Configure(PrismAppBuilder builder)
    {
        builder.ConfigureModuleCatalog(ConfigureModuleCatalog);
        builder.RegisterTypes(RegisterTypes)
            .AddGlobalNavigationObserver(context => context.Subscribe(x =>
            {
                if (x.Type == NavigationRequestType.Navigate)
                    Console.WriteLine($"Navigation: {x.Uri}");
                else
                    Console.WriteLine($"Navigation: {x.Type}");

                var status = x.Cancelled ? "Cancelled" : x.Result.Success ? "Success" : "Failed";
                Console.WriteLine($"Result: {status}");

                if (status == "Failed" && !string.IsNullOrEmpty(x.Result?.Exception?.Message))
                    Console.Error.WriteLine(x.Result.Exception.Message);
            }))
            .OnAppStart(nameof(ApplicationShellView));
    }

    private static void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<AppUIModule>();
        moduleCatalog.AddModule<LoginModule>();
    }
    private static void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<ApplicationShellView, ApplicationShellViewModel>();
    }
}

