using LibraryAPI.Domain.Entities;
using LibraryAPI.Domain.Enums;
using Xunit;

namespace LibraryAPI.Application.Tests.Domain.Entities;

public class BookTests
{
    [Fact]
    public void Constructor_Should_Create_Book_With_Valid_Data()
    {
        var id = Guid.NewGuid();
        var title = "Test Book";
        var author = "Test Author";
        var publicationYear = 2024;
        var genre = "Fiction";
        var status = BookStatus.Available;

        var book = new Book(id, title, author, publicationYear, genre, status)
        {
            Title = title,
            Author = author,
            Genre = genre
        };

        Assert.Equal(id, book.Id);
        Assert.Equal(title, book.Title);
        Assert.Equal(author, book.Author);
        Assert.Equal(publicationYear, book.PublicationYear);
        Assert.Equal(genre, book.Genre);
        Assert.Equal(status, book.Status);
    }

    [Fact]
    public void Borrow_Should_Change_Status_To_Borrowed()
    {
        var book = new Book(Guid.NewGuid(), "Test Book", "Test Author", 2024, "Fiction", BookStatus.Available)
        {
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction"
        };

        book.Borrow();

        Assert.Equal(BookStatus.Borrowed, book.Status);
    }

    [Fact]
    public void Return_Should_Change_Status_To_Available()
    {
        var book = new Book(Guid.NewGuid(), "Test Book", "Test Author", 2024, "Fiction", BookStatus.Borrowed)
        {
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction"
        };

        book.Return();

        Assert.Equal(BookStatus.Available, book.Status);
    }

    [Fact]
    public void UpdateDetails_Should_Update_Book_Properties()
    {
        var book = new Book(Guid.NewGuid(), "Old Title", "Old Author", 2023, "Old Genre", BookStatus.Available)
        {
            Title = "Old Title",
            Author = "Old Author",
            Genre = "Old Genre"
        };
        var newTitle = "New Title";
        var newAuthor = "New Author";
        var newYear = 2024;
        var newGenre = "New Genre";

        book.UpdateDetails(newTitle, newAuthor, newYear, newGenre);

        Assert.Equal(newTitle, book.Title);
        Assert.Equal(newAuthor, book.Author);
        Assert.Equal(newYear, book.PublicationYear);
        Assert.Equal(newGenre, book.Genre);
    }
}