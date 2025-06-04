using LibraryAPI.Application.DTOs.Auth;

namespace LibraryAPI.Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    string GenerateJwtToken(string username, string role);
} 