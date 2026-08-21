using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Models.Check;
using SentinelApi.Monitoring.Models.Check;
using SentinelLib.Identity.Security.CurrentUser;
using System.Text.Json;

namespace SentinelApi.Monitoring.Controllers;

/// <summary>
/// Адміністрування перевірок сервісів.
/// </summary>
/// <remarks>
/// [Auth]
/// </remarks>
[ApiController]
[Authorize]
[Route("api/check")]
public class CheckController(
    ICheckService checkService,
    ICheckExecutor checkExecutor,
    IHistoryService historyService,
    ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    private readonly ICheckExecutor _checkExecutor = checkExecutor;
    private readonly ICheckService _checkService = checkService;
    private readonly IHistoryService _historyService = historyService;
    private readonly ICurrentUserAccessor _currentUserAccessor = currentUserAccessor;

    /// <summary>
    /// Отримання інформації про перевірку.
    /// </summary>
    [HttpGet("get")]
    [ProducesResponseType(typeof(CheckDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] int id, CancellationToken ct)
    {
        var checkDetails = await _checkService.GetAsync(id, ct);

        return Ok(checkDetails);
    }

    /// <summary>
    /// Отримання списку перевірок.
    /// </summary>
    [HttpGet("get/list")]
    [ProducesResponseType(typeof(GetCheckListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList(CancellationToken ct)
    {
        var checkList = await _checkService.GetListAsync(ct);

        return Ok(checkList);
    }

    /// <summary>
    /// Створення параметрів перевірки.
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCheckModel model, CancellationToken ct)
    {
        var checkId = await _checkService.CreateAsync(new CreateCheckRequest(
            ServiceDefinitionId: model.ServiceDefinitionId,
            Name: model.Name,
            EndpointUrl: model.EndpointUrl,
            Description: model.Description,
            ProbeType: model.ProbeType,
            IsEnabled: model.IsEnabled
        ), ct);

        // logging
        var userId = _currentUserAccessor.UserId.ToString();
        var userLogin = _currentUserAccessor.Login;
        var modelJson = JsonSerializer.Serialize(model);
        await _historyService.SaveCheckCreateActionLogAsync(userId, userLogin, checkId.ToString(), model.Name, modelJson, ct);

        return Ok();
    }

    /// <summary>
    /// Оновлення параметрів перевірки.
    /// </summary>
    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateCheckModel model, CancellationToken ct)
    {
        await _checkService.UpdateAsync(new UpdateCheckRequest(
            Id: model.Id,
            Name: model.Name,
            EndpointUrl: model.EndpointUrl,
            Description: model.Description,
            ProbeType: model.ProbeType,
            IsEnabled: model.IsEnabled
        ), ct);

        // логування змін відбувається інтерсептором при запиті до БД

        return Ok();
    }

    /// <summary>
    /// Ручний запуск перевірки.
    /// </summary>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Start([FromBody] int id, CancellationToken ct)
    {
        await _checkExecutor.ExecuteManualAsync(id, ct);

        // logging
        var userId = _currentUserAccessor.UserId.ToString();
        var userLogin = _currentUserAccessor.Login;
        var checkName = await _checkService.GetNameByIdAsync(id, ct);
        await _historyService.SaveServiceCheckTriggerActionLogAsync(userId, userLogin, id.ToString(), checkName, string.Empty, ct);

        return Ok();
    }
}
