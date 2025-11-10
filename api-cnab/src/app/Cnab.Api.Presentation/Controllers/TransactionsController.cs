using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cnab.Api.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender) => _sender = sender;

    /// <summary>
    /// Returns all imported transactions with store information.
    /// </summary>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public async Task<IActionResult> GetTransactions(CancellationToken ct)
    {
        try
        {
            var list = await _sender.Send(new GetAllTransactionsQuery(), ct);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Post()
    {
        try
        {
            if (!Request.HasFormContentType || Request.ContentLength == 0)
                return BadRequest("Request must be multipart/form-data with a file.");

            var form = await Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();
            if (file == null) return BadRequest("file is required");

            var published = await _sender.Send(new PublishCnabFileCommand(file));
            return Ok(new { published });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
