
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Models.History;
using SentinelApi.Monitoring.Models.History;

namespace SentinelApi.Monitoring.Controllers;

/// <summary>
/// Отримання історії запитів користувачів сервісу.
/// </summary>
/// <remarks>
/// [Auth]
/// </remarks>
[ApiController]
[Authorize]
[Route("api/history")]
public class HistoryController(IHistoryService historyService) : ControllerBase
{
    private readonly IHistoryService _historyService = historyService;

    /// <summary>
    /// Отримання списку запитів користувачів сервісу.
    /// </summary>
    [HttpGet("get/list")]
    [ProducesResponseType(typeof(GetActionLogListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList([FromQuery] GetActionHistoryListModel model, CancellationToken ct)
    {
        var actionLogList = await _historyService.GetListAsync(new GetActionLogListRequest(
            SearchParam: model.SearchParam,
            Page: model.Page,
            PageSize: model.PageSize
        ), ct);

        return Ok(actionLogList);
    }
}
