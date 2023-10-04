using System;
using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using XTConnect.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace XTConnect.Core.Services;

public class XTConnectDataService(HttpClient httpClient, IConfiguration configuration) : IXTConnectDataService
{
    private const string TokenKey = "AuthToken"; // Key to store token in SecureStorage
    private readonly string _loginUrl = configuration["ServiceUrls:LoginUrl"] ?? string.Empty;

    public async Task<bool> Authenticate(string username, string password)
    {
        var credentials = new
        {
            userName = username,
            password = password
        };

        var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json-patch+json");
        var response = await httpClient.PostAsync(_loginUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());

            if (tokenResponse?.Token == null)
            {
                throw new Exception("No token received from the server.");
            }

            await SecureStorage.SetAsync(TokenKey, tokenResponse.Token);

            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.Token);

            return true;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw new Exception("Invalid username or password.");
            case HttpStatusCode.InternalServerError:
                throw new Exception("Server error. Please try again later.");
            default:
                throw new Exception($"Unexpected status code: {response.StatusCode}");
        }
    }

    public async Task<bool> IsAuthenticated()
    {
        var existingToken = await SecureStorage.GetAsync(TokenKey);

        if (string.IsNullOrEmpty(existingToken))
        {
            return false;
        }

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(existingToken);

        if (token.ValidTo < DateTime.UtcNow)
        {
            SecureStorage.Remove(TokenKey);
            return false;
        }

        return true;

    }

    public async Task<T> GetData<T>(string url)
    {
        await CheckAndRefreshToken();

        var response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content)!;
        }

        return default!;
    }

    public async Task<bool> PostData<T>(string url, T data)
    {
        await CheckAndRefreshToken();

        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(url, content);
        return response.IsSuccessStatusCode;
    }
    
    private async Task CheckAndRefreshToken()
    {
        // Check if there's an existing token
        var existingToken = await SecureStorage.GetAsync(TokenKey);
        if (!string.IsNullOrEmpty(existingToken))
        {
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", existingToken);
        }
        else
        {
            // If there's no token, you could either throw an exception or attempt to re-authenticate
            throw new Exception("No authentication token found.");
        }
    }
}

public class TokenResponse
{
    public TokenResponse(string token)
    {
        Token = token;
    }

    public string Token { get; set; }
    // Add other properties as per the actual response
}