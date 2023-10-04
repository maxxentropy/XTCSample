using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Prism.AppModel;
using Prism.Common;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Regions;

namespace XTConnect.Core.Mvvm;

public abstract class ViewModelBase : BindableBase, IInitialize, IRegionAware, IPageLifecycleAware
{
    protected ViewModelBase(BaseServices baseServices)
    {
        NavigationService = baseServices.NavigationService;
        PageAccessor = baseServices.PageAccessor;
        
        Title = Regex.Replace(GetType().Name, "ViewModel", string.Empty);
        
        Messages = new ObservableCollection<string>();
        Messages.CollectionChanged += (sender, args) =>
        {
            if (args.NewItems != null)
                foreach (string message in args.NewItems)
                    Console.WriteLine($"{Title} - {message}");
        };
    }

    protected string Name => GetType().Name.Replace("ViewModel", string.Empty);
    protected INavigationService NavigationService { get; }
    private IPageAccessor PageAccessor { get; }
    protected IRegionNavigationService? RegionNavigation { get; private set; }

    
    
    public ObservableCollection<string> Messages { get; }
    protected string Title { get; init; }
    
    public void Initialize(INavigationParameters parameters)
    {
        Messages.Add("ViewModel Initialized");
        foreach (var parameter in parameters.Where(x => x.Key.Contains("message")))
        {
            string? empty = string.Empty;
            Messages.Add(parameter.Value.ToString() ?? empty);
        }
    }
   
    private string? _message;
    public string? Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public string? PageName => PageAccessor.Page?.GetType()?.Name;
    
    private int _viewCount;
    public int ViewCount
    {
        get => _viewCount;
        set => SetProperty(ref _viewCount, value);
    }

    public void OnAppearing()
    {
        RaisePropertyChanged(nameof(PageName));
    }

    public void OnDisappearing()
    {
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey(nameof(Message)))
            Message = navigationContext.Parameters.GetValue<string>(nameof(Message));

        RegionNavigation = navigationContext.NavigationService;
        ViewCount = navigationContext.NavigationService.Region.Views.Count();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return navigationContext.NavigatedName() == Name;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        if (navigationContext.Parameters.ContainsKey(nameof(Message)))
            Message = navigationContext.Parameters.GetValue<string>(nameof(Message));
    }
}
