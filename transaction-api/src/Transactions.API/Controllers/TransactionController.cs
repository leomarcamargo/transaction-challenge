using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transactions.Application.Commands.CreateTransaction;
using Transactions.Application.Queries.GetAllTransactions;
using Transactions.Application.Queries.GetTransactionById;

namespace Transactions.API.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new transaction", Description = "Creates a new transaction and returns the created transaction with its ID.")]
    [SwaggerResponse(201, "Transaction created successfully", typeof(CreateTransactionResult))]
    [SwaggerResponse(400, "Invalid input")]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(GetTransactionById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Gets a transaction by ID", Description = "Retrieves a transaction by its unique ID.")]
    [SwaggerResponse(200, "Transaction found", typeof(GetTransactionByIdResult))]
    [SwaggerResponse(404, "Transaction not found")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var query = new GetTransactionByIdQuery(id);
        var result = await mediator.Send(query);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all transactions with pagination", Description = "Retrieves a paginated list of transactions.")]
    [SwaggerResponse(200, "Returns the list of transactions", typeof(GetAllTransactionsResult))]
    [SwaggerResponse(400, "Invalid pagination parameters")]
    public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetAllTransactionsQuery(pageNumber, pageSize);
        var result = await mediator.Send(query);
        return Ok(result);
    }
}