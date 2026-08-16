using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Models;

namespace SentinelApi.Identity.Controllers;

/// <summary>
/// Автентифікація користувачів.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Виконує вхід за логіном і паролем та видає токен доступу.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct)
    {
        var response = await authService.LoginAsync(new LoginCommand(request.Login, request.Password), ct);

        return Ok(response);
    }
}
