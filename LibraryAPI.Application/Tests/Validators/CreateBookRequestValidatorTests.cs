using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Validators;
using Xunit;

namespace LibraryAPI.Application.Tests.Validators;

public class CreateBookRequestValidatorTests
{
    private readonly CreateBookRequestValidator _validator;

    public CreateBookRequestValidatorTests()
    {
        _validator = new CreateBookRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new CreateBookRequest
        {
            Title = string.Empty,
            Author = "Test Author",
            PublicationYear = 2024,
            Genre = "Fiction"
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_Author_Is_Empty()
    {
        var model = new CreateBookRequest
        {
            Title = "Test Book",
            Author = string.Empty,
            PublicationYear = 2024,
            Genre = "Fiction"
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Author");
    }

    [Fact]
    public void Should_Have_Error_When_PublicationYear_Is_Invalid()
    {
        var model = new CreateBookRequest
        {
            Title = "Test Book",
            Author = "Test Author",
            PublicationYear = 0,
            Genre = "Fiction"
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PublicationYear");
    }

    [Fact]
    public void Should_Have_Error_When_Genre_Is_Empty()
    {
        var model = new CreateBookRequest
        {
            Title = "Test Book",
            Author = "Test Author",
            PublicationYear = 2024,
            Genre = string.Empty
        };

        var result = _validator.Validate(model);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Genre");
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var model = new CreateBookRequest
        {
            Title = "Test Book",
            Author = "Test Author",
            PublicationYear = 2024,
            Genre = "Fiction"
        };

        var result = _validator.Validate(model);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }
}