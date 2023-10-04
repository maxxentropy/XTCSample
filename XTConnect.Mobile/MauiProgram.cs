using Microsoft.Extensions.Logging;
using XTConnect.Mobile.Extensions;

namespace XTConnect.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			//
			//
			//
			.UseMauiApp<App>()
			.UsePrism(PrismStartup.Configure)
			.ConfigureAppSettings("XTConnect.Mobile.appsettings.json")
			.SetupSerilog()
			.UseVLinkDataService()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
