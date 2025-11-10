using Cnab.Api.Application.Stores.Queries.GetAllStores;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.Api.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class StoresController : ControllerBase
{
    private readonly ISender _sender;

    public StoresController(ISender sender) => _sender = sender;

    /// <summary>
    /// Returns a list of stores with the aggregated balance of their transactions.
    /// </summary>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public async Task<IActionResult> GetStores(CancellationToken ct)
    {
        try
        {
            var list = await _sender.Send(new GetAllStoresQuery(), ct);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
