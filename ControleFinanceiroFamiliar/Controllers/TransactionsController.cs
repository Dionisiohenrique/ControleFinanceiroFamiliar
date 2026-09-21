using ControleFinanceiroFamiliar.Application.Contracts.TransactionDtos;
using ControleFinanceiroFamiliar.Application.UseCases.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiroFamiliar.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    public TransactionsController(ILogger<TransactionsController> logger, TransactionService transactionService)
    {
        _logger = logger;
        _transactionService = transactionService;
    }

    private readonly ILogger<TransactionsController> _logger;
    private readonly TransactionService _transactionService;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to, [FromQuery] Guid? memberId, CancellationToken ct)
    {
        var result = await _transactionService.ListAsync(from, to, memberId, ct);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request, CancellationToken ct)
    {
        var result = await _transactionService.CreateAsync(request, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(List), null, result.Value) 
            : BadRequest(new {errors = result.Errors});
    }

    [HttpDelete("{id")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _transactionService.DeleteAsync(id, ct);
        return result.IsSuccess ? NoContent() : BadRequest(new {errors = result.Errors});
    }
}