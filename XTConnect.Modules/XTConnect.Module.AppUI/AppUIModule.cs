using XTConnect.Core.Constants;
using XTConnect.Module.AppUI.ViewModels;
using XTConnect.Module.AppUI.Views;

namespace XTConnect.Module.AppUI;

// All the code in this file is included in all platforms.
public class AppUIModule: IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion(PrismRegionNavConstants.ContentRegion, nameof(GatewayView));
    }
    
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry
            .RegisterForRegionNavigation<GatewayView, GatewayViewModel>();
    }
}