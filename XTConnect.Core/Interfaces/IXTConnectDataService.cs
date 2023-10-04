using System.Threading.Tasks;

namespace XTConnect.Core.Interfaces;

public interface IXTConnectDataService
{
    Task<bool> Authenticate(string username, string password);
    Task<bool> IsAuthenticated();
    Task<T> GetData<T>(string url);
    Task<bool> PostData<T>(string url, T data);
}