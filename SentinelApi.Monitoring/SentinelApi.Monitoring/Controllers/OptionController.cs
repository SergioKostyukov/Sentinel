using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Models.Option;

namespace SentinelApi.Monitoring.Controllers;

/// <summary>
/// Отримання опцій для полів вибору.
/// </summary>
/// <remarks>
/// [Auth]
/// </remarks>
[ApiController]
[Authorize]
[Route("api/option")]
public class OptionController(IOptionService optionService) : ControllerBase
{
    private readonly IOptionService _optionService = optionService;

    /// <summary>
    /// Отримання опцій зарестрованих сервісів.
    /// </summary>
    [HttpGet("get/service-definition")]
    [ProducesResponseType(typeof(List<OptionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServiceDefinitionOptions(CancellationToken ct)
    {
        var serviceDefinitionOptions = await _optionService.GetServiceDefinitionsAsync(ct);

        return Ok(serviceDefinitionOptions);
    }
}
