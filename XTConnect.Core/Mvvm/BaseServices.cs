using Prism.Common;
using Prism.Navigation;
using Prism.Services;

namespace XTConnect.Core.Mvvm;

public class BaseServices(INavigationService navigationService,
    IPageDialogService pageDialogs,
    IPageAccessor pageAccessor)
{
    public INavigationService NavigationService { get; } = navigationService;
    public IPageDialogService PageDialogs { get; } = pageDialogs;
    public IPageAccessor PageAccessor { get; } = pageAccessor;
}