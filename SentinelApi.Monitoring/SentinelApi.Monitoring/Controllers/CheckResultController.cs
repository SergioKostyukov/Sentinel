using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SentinelApi.Monitoring.Application.Interfaces;
using SentinelApi.Monitoring.Application.Models.CheckResult;
using SentinelApi.Monitoring.Models.CheckResult;

namespace SentinelApi.Monitoring.Controllers;

/// <summary>
/// Отримання історії результатів перевірок.
/// </summary>
/// <remarks>
/// [Auth]
/// </remarks>
[ApiController]
[Authorize]
[Route("api/check-result")]
public class CheckResultController(ICheckResultService checkResultService) : ControllerBase
{
    private readonly ICheckResultService _checkResultService = checkResultService;

    /// <summary>
    /// Отримання результатів перевірки.
    /// </summary>
    [HttpGet("get")]
    [ProducesResponseType(typeof(CheckResultDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] int id, CancellationToken ct)
    {
        var checkResultList = await _checkResultService.GetAsync(id, ct);

        return Ok(checkResultList);
    }

    /// <summary>
    /// Отримання списку результатів перевірок.
    /// </summary>
    [HttpGet("get/list")]
    [ProducesResponseType(typeof(GetCheckResultListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetList([FromQuery] GetCheckResultListModel model, CancellationToken ct)
    {
        var checkResultList = await _checkResultService.GetListAsync(new GetCheckResultListRequest(
            SearchParam: model.SearchParam,
            SearchDate: model.SearchDate,
            Page: model.Page,
            PageSize: model.PageSize
        ), ct);

        return Ok(checkResultList);
    }
}
