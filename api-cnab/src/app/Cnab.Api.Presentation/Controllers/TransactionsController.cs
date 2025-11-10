using Cnab.Api.Application.Transactions.Command.PublishCnabFile;
using Cnab.Api.Application.Transactions.Queries.GetAllTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cnab.Api.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ISender sender, ILogger<TransactionsController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    /// Returns all imported transactions with store information.
    /// </summary>
    [HttpGet]
    [MapToApiVersion("1.0")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    public async Task<IActionResult> GetTransactions(CancellationToken ct)
    {
        _logger.LogInformation("GetTransactions called");
        try
        {
            var list = await _sender.Send(new GetAllTransactionsQuery(), ct);
            _logger.LogInformation("GetTransactions returned {Count} items", (list as IEnumerable<object>)?.Count() ?? 0);
            return Ok(list);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Bad request in GetTransactions");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in GetTransactions");
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
        _logger.LogInformation("Upload endpoint called");
        try
        {
            if (!Request.HasFormContentType || Request.ContentLength == 0)
            {
                _logger.LogWarning("Upload request missing form content or empty content length");
                return BadRequest("Request must be multipart/form-data with a file.");
            }

            var form = await Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();
            if (file == null)
            {
                _logger.LogWarning("Upload request did not contain a file");
                return BadRequest("file is required");
            }

            var published = await _sender.Send(new PublishCnabFileCommand(file));
            _logger.LogInformation("Published {Count} lines from uploaded file {FileName}", published, file.FileName);
            return Ok(new { published });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Bad request in upload");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error in upload");
            return Problem(ex.Message);
        }
    }
}
