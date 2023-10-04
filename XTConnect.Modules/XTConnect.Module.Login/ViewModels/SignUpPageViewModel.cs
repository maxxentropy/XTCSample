using System.Windows.Input;
using XTConnect.Core.Mvvm;
using XTConnect.Module.Login.Views;

namespace XTConnect.Module.Login.ViewModels;

public class SignUpPageViewModel : ViewModelBase
{
    public ICommand SignInCommand { get; private set; }
    private IRegionManager _regionManager;
    
    public SignUpPageViewModel(BaseServices baseServices, IRegionManager regionManager)
        : base(baseServices)
    {
        SignInCommand = new Command(async () => await SignInAsync());
        _regionManager = regionManager;
    }

    private async Task SignInAsync()
    {
        await Task.Run(() => _regionManager.RequestNavigate("ContentRegion", nameof(SignInPageView)));
        //var navResult = await _navigationService.NavigateAsync(nameof(SignInPageView));
    }
}