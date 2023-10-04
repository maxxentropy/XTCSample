using XTConnect.Core.Constants;
namespace XTConnect.Mobile.ViewModels;
public class ApplicationShellViewModel : IInitialize
{
    private IRegionManager RegionManager { get; }
    
    public ApplicationShellViewModel(IRegionManager regionManager)
    {
        RegionManager = regionManager;
        NavigateCommand = new DelegateCommand<string>(OnNavigateCommandExecuted);
    }

    public DelegateCommand<string> NavigateCommand { get; }
    
    public void Initialize(INavigationParameters parameters)
    {
        //throw new NotImplementedException();
        //RegionManager.RequestNavigate(PrismRegionNavConstants.ContentRegion, "LoginPageView");
    }
   
    private void OnNavigateCommandExecuted(string uri)
    {
        RegionManager.RequestNavigate(PrismRegionNavConstants.ContentRegion, uri);
    }
}