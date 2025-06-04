using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Features.Books.Commands;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using Moq;
using Xunit;

namespace LibraryAPI.Application.Tests.Features.Books.Commands;

public class CreateBookCommandTests
{
    private readonly Mock<IBookRepository> _bookRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateBookCommandHandler _handler;

    public CreateBookCommandTests()
    {
        _bookRepositoryMock = new Mock<IBookRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateBookCommandHandler(_bookRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Book_When_Valid()
    {
        var request = new CreateBookRequest
        {
            Title = "Test Book",
            Author = "Test Author",
            PublicationYear = 2024,
            Genre = "Fiction"
        };

        var book = new Book(
            Guid.NewGuid(),
            request.Title,
            request.Author,
            request.PublicationYear,
            request.Genre,
            BookStatus.Available
        )
        {
            Title = request.Title,
            Author = request.Author,
            Genre = request.Genre
        };

        var bookDto = new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            Status = book.Status
        };

        _mapperMock
            .Setup(x => x.Map<Book>(request))
            .Returns(book);

        _mapperMock
            .Setup(x => x.Map<BookDto>(book))
            .Returns(bookDto);

        _bookRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var command = new CreateBookCommand(request);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(bookDto.Id, result.Id);
        Assert.Equal(bookDto.Title, result.Title);
        Assert.Equal(bookDto.Author, result.Author);
        Assert.Equal(bookDto.PublicationYear, result.PublicationYear);
        Assert.Equal(bookDto.Genre, result.Genre);
        Assert.Equal(bookDto.Status, result.Status);

        _bookRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}