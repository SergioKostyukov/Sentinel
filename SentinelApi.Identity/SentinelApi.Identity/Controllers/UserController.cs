using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Identity.Application.Contracts;
using SentinelApi.Identity.Application.Interfaces;
using SentinelApi.Identity.Models;

namespace SentinelApi.Identity.Controllers;

/// <summary>
/// Керування користувачами системи.
/// </summary>
[ApiController]
[Authorize]
[Route("api/user")]
public sealed class UserController(IUserManagementService userManagementService) : ControllerBase
{
    /// <summary>
    /// Повертає список усіх користувачів.
    /// </summary>
    [HttpGet("get/list")]
    [ProducesResponseType(typeof(IReadOnlyList<UserSummaryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<UserSummaryResponse>>> GetUserList(CancellationToken ct)
    {
        var users = await userManagementService.GetUsersAsync(ct);

        return Ok(users);
    }

    /// <summary>
    /// Створює нового користувача.
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser(CreateUserRequest request, CancellationToken ct)
    {
        await userManagementService.CreateUserAsync(new CreateUserCommand(request.Login, request.Email, request.Password), ct);

        return Created();
    }
}
