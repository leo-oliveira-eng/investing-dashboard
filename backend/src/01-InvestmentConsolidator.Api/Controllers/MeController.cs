using Microsoft.AspNetCore.Mvc;

namespace InvestmentConsolidator.Api.Controllers;

[ApiController, Route("echo")]
public class MeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Ok(new { name = "Investment Consolidator", version = "0.1.0" });
    }
}
