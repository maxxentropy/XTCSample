using XTConnect.Core.Constants;
using XTConnect.Module.Login.ViewModels;
using XTConnect.Module.Login.Views;

namespace XTConnect.Module.Login;

// All the code in this file is included in all platforms.
public class LoginModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion(PrismRegionNavConstants.ContentRegion, nameof(SignInPageView));
        regionManager.RegisterViewWithRegion(PrismRegionNavConstants.ContentRegion, nameof(SignUpPageView));
    }
    
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry
            .RegisterForRegionNavigation<SignInPageView, SignInPageViewModel>()
            .RegisterForRegionNavigation<SignUpPageView, SignUpPageViewModel>();
    }
}
