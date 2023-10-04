using System.Collections;
using System.ComponentModel;
using XTConnect.Core.Constants;
using XTConnect.Core.Interfaces;
using XTConnect.Core.Mvvm;

namespace XTConnect.Module.Login.ViewModels;

public class SignInPageViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private string _userName;
    private string _password;
    private IRegionManager _regionManager;
    private IXTConnectDataService _dataService;
    private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
    
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    public bool HasErrors => _validationErrors.Any();

    public DelegateCommand<string> SignUpCommand { get; private set; }
    public DelegateCommand<string> SignInCommand { get; private set; }
    public DelegateCommand<string> VLinkAppCommand { get; private set; }

    public SignInPageViewModel(BaseServices baseServices, IRegionManager regionManager, IXTConnectDataService dataService)
        : base(baseServices)
    {
        Title = "Sign In Page";
        SignUpCommand = new DelegateCommand<string>(async (u) => await SignUpAsync(u));
        SignInCommand = new DelegateCommand<string>(async (u) => await SignInAsync());
        VLinkAppCommand = new DelegateCommand<string>(async (u) => await NavToAppAsync(u));
        _regionManager = regionManager;
        _dataService = dataService;
    }

    //public string Title { get; }
    public string Username
    {
        get => _userName;
        set
        {
            _userName = value;
            ValidateUsername();
            RaisePropertyChanged(nameof(Username));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            ValidatePassword();
            RaisePropertyChanged();
        }
    }
    private async Task SignInAsync()
    {
        try
        {
            if (await _dataService.Authenticate(Username, Password))
                await NavigationService.GoBackAsync();
                //_regionManager.RequestNavigate("ContentRegion", "AppContentPageView");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private async Task SignUpAsync(string uri)
    {
        if (uri == string.Empty)
            uri = nameof(SignUpCommand);

        await Task.Run(() => _regionManager.RequestNavigate(PrismRegionNavConstants.ContentRegion, uri));
    }
    private async Task NavToAppAsync(string uri)
    {
        if (uri == string.Empty)
            uri = nameof(SignUpCommand);

        await Task.Run(() => _regionManager.RequestNavigate(PrismRegionNavConstants.ContentRegion, uri));
    }
    public IEnumerable GetErrors(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
            return null;

        return _validationErrors[propertyName];
    }

    private void ValidateUsername()
    {
        ClearErrors(nameof(Username));
        if (string.IsNullOrWhiteSpace(Username))
            AddError(nameof(Username), "Username cannot be empty.");
        // Additional username validation logic here...
    }

    private void ValidatePassword()
    {
        ClearErrors(nameof(Password));
        if (string.IsNullOrWhiteSpace(Password))
            AddError(nameof(Password), "Password cannot be empty.");
        // Additional password validation logic here...
    }

    private void AddError(string propertyName, string error)
    {
        if (!_validationErrors.ContainsKey(propertyName))
            _validationErrors[propertyName] = new List<string>();

        _validationErrors[propertyName].Add(error);
        OnErrorsChanged(propertyName);
    }

    private void ClearErrors(string propertyName)
    {
        if (_validationErrors.ContainsKey(propertyName))
            _validationErrors.Remove(propertyName);

        OnErrorsChanged(propertyName);
    }
    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}