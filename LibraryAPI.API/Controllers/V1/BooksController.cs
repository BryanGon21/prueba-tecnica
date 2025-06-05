using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.DTOs.Auth;
using LibraryAPI.Application.Features.Books.Commands;
using LibraryAPI.Application.Features.Books.Queries;
using LibraryAPI.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.API.Controllers.V1;

/// <summary>
/// Controller for managing books in the library
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BooksController> _logger;

    public BooksController(IMediator mediator, ILogger<BooksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all books in the library
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of books</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<BookDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<BookDto>>> GetAll(CancellationToken ct)
    {
        _logger.LogInformation("Retrieving all books");
        var result = await _mediator.Send(new GetAllBooksQuery(), ct);
        _logger.LogInformation("{Count} books found", result.Count);
        return Ok(result);
    }

    /// <summary>
    /// Gets a book by its ID
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Book details</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookDto>> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Searching for book with ID: {Id}", id);
        var result = await _mediator.Send(new GetBookByIdQuery(id), ct);
        if (result == null)
        {
            _logger.LogWarning("Book with ID: {Id} not found", id);
            throw new NotFoundException("Book not found");
        }
        _logger.LogInformation("Book found: {Title}", result.Title);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new book
    /// </summary>
    /// <param name="request">Book creation request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created book</returns>
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Creating new book: {Title}", request.Title);
        var result = await _mediator.Send(new CreateBookCommand(request), ct);
        _logger.LogInformation("Book created with ID: {Id}", result.Id);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="request">Book update request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Updating book with ID: {Id}", id);
        var result = await _mediator.Send(new UpdateBookCommand(id, request), ct);
        if (!result)
        {
            _logger.LogWarning("Book to update with ID: {Id} not found", id);
            throw new NotFoundException("Book not found");
        }
        _logger.LogInformation("Book updated successfully");
        return NoContent();
    }

    /// <summary>
    /// Deletes a book
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Deleting book with ID: {Id}", id);
        var result = await _mediator.Send(new DeleteBookCommand(id), ct);
        if (!result)
        {
            _logger.LogWarning("Book to delete with ID: {Id} not found", id);
            throw new NotFoundException("Book not found");
        }
        _logger.LogInformation("Book deleted successfully");
        return NoContent();
    }

    /// <summary>
    /// Marks a book as borrowed
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpPatch("{id}/borrow")]
    [Authorize(Roles = "user,admin")]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Borrow([FromRoute] Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Borrowing book with ID: {Id}", id);
        var result = await _mediator.Send(new BorrowBookCommand(id), ct);
        if (!result)
        {
            _logger.LogWarning("Book to borrow with ID: {Id} not found", id);
            throw new NotFoundException("Book not found");
        }
        _logger.LogInformation("Book borrowed successfully");
        return NoContent();
    }

    /// <summary>
    /// Marks a book as returned
    /// </summary>
    /// <param name="id">Book ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>No content if successful</returns>
    [HttpPatch("{id}/return")]
    [Authorize(Roles = "user,admin")]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(AuthErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Return([FromRoute] Guid id, CancellationToken ct)
    {
        _logger.LogInformation("Returning book with ID: {Id}", id);
        var result = await _mediator.Send(new ReturnBookCommand(id), ct);
        if (!result)
        {
            _logger.LogWarning("Book to return with ID: {Id} not found", id);
            throw new NotFoundException("Book not found");
        }
        _logger.LogInformation("Book returned successfully");
        return NoContent();
    }
}