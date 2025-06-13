using OnlineEducationPlatform.API.Models.Identity;

namespace OnlineEducationPlatform.API.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public IdentityService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["Services:IdentityService"]);
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TokenResponse>();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new UnauthorizedAccessException(error);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", refreshToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TokenResponse>();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new UnauthorizedAccessException(error);
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/validate-token", token);
        return response.IsSuccessStatusCode;
    }
}