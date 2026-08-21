using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Models.ServiceDefinition;
using SentinelApi.Monitoring.Models.ServiceDefinition;
using SentinelLib.Identity.Security.CurrentUser;
using System.Text.Json;

namespace SentinelApi.Monitoring.Controllers;

/// <summary>
/// Адміністрування даних про сервіси, які моніторяться.
/// </summary>
/// <remarks>
/// [Auth]
/// </remarks>
[ApiController]
[Authorize]
[Route("api/service-definition")]
public class ServiceDefinitionController(
    IServiceDefinitionService serviceDefinitionService,
    IHistoryService historyService,
    ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    private readonly IServiceDefinitionService _serviceDefinitionService = serviceDefinitionService;
    private readonly IHistoryService _historyService = historyService;
    private readonly ICurrentUserAccessor _currentUserAccessor = currentUserAccessor;

    /// <summary>
    /// Отримання інформації про сервіс.
    /// </summary>
    [HttpGet("get")]
    [ProducesResponseType(typeof(ServiceDefinitionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] int id, CancellationToken ct)
    {
        var serviceDefinition = await _serviceDefinitionService.GetAsync(id, ct);

        return Ok(serviceDefinition);
    }

    /// <summary>
    /// Отримання списку сервісів.
    /// </summary>
    [HttpGet("get/list")]
    [ProducesResponseType(typeof(GetServiceDefinitionListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList(CancellationToken ct)
    {
        var serviceDefinitionList = await _serviceDefinitionService.GetListAsync(ct);

        return Ok(serviceDefinitionList);
    }

    /// <summary>
    /// Заповнення інформації про сервіс.
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateServiceDefinitionModel model, CancellationToken ct)
    {
        var serviceDefinitionId = await _serviceDefinitionService.CreateAsync(new CreateServiceDefinitionRequest(
            Name: model.Name,
            Url: model.Url,
            NotificationEmails: model.NotificationEmails,
            Description: model.Description
        ), ct);

        // logging
        var userId = _currentUserAccessor.UserId.ToString();
        var userLogin = _currentUserAccessor.Login;
        var modelJson = JsonSerializer.Serialize(model);
        await _historyService.SaveServiceDefinitionCreateActionLogAsync(userId, userLogin, serviceDefinitionId.ToString(), model.Name, modelJson, ct);

        return Ok();
    }

    /// <summary>
    /// Оновлення інформації про сервіс.
    /// </summary>
    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateServiceDefinitionModel model, CancellationToken ct)
    {
        await _serviceDefinitionService.UpdateAsync(new UpdateServiceDefinitionRequest(
            Id: model.Id,
            Name: model.Name,
            Url: model.Url,
            NotificationEmails: model.NotificationEmails,
            Description: model.Description
        ), ct);

        // логування змін відбувається інтерсептором при запиті до БД

        return Ok();
    }
}
