using XTConnect.Core.Constants;
using XTConnect.Core.Interfaces;
using XTConnect.Core.Mvvm;

namespace XTConnect.Module.AppUI.ViewModels;

public class GatewayViewModel: ViewModelBase
{
    private readonly IRegionManager _regionManager;
    private readonly IXTConnectDataService _dataService;
    
    public GatewayViewModel(BaseServices baseServices, IRegionManager regionManager, IXTConnectDataService dataService)
        : base(baseServices)
    {
        _regionManager = regionManager;
        _dataService = dataService;
        CheckAuthenticationState();
    }
    
    public bool IsAuthenticated { get; set; }
    
    private async void CheckAuthenticationState()
    {
        if (!await _dataService.IsAuthenticated())
        {
            await NavigateToLogin();
        }
    }
    private async Task NavigateToLogin()
    {
        await Task.FromResult(() => _regionManager.RequestNavigate(PrismRegionNavConstants.ContentRegion, "GatewayView/SignInPageView"));
    }
}