using LibraryAPI.Application.DTOs.Auth;
using LibraryAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers.V1;

/// <summary>
/// Controller for handling authentication operations
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="request">Login credentials containing username and password</param>
    /// <returns>JWT token and user information if authentication is successful</returns>
    /// <response code="200">Returns the JWT token and user information</response>
    /// <response code="401">If the credentials are invalid</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for user: {Username}", request.Username);
            var response = await _authService.LoginAsync(request);
            _logger.LogInformation("Successful login for user: {Username}", request.Username);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Failed login for user: {Username}", request.Username);
            return Unauthorized(new AuthErrorResponse(ex.Message));
        }
    }
}