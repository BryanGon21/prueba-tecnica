using Ardalis.GuardClauses;
using LibraryAPI.Domain.Enums;

namespace LibraryAPI.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; }
        public BookStatus Status { get; set; }

        public Book()
        {
            Id = Guid.NewGuid();
            Status = BookStatus.Available;
        }

        public Book(Guid id, string title, string author, int publicationYear, string genre, BookStatus status = BookStatus.Available)
        {
            Id = id;
            Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Author = Guard.Against.NullOrWhiteSpace(author, nameof(author));
            PublicationYear = Guard.Against.NegativeOrZero(publicationYear, nameof(publicationYear));
            Genre = Guard.Against.NullOrWhiteSpace(genre, nameof(genre));
            Status = status;
        }

        public void Borrow()
        {
            if (Status == BookStatus.Borrowed)
                throw new InvalidOperationException("Book is already borrowed.");
            Status = BookStatus.Borrowed;
        }

        public void Return()
        {
            if (Status == BookStatus.Available)
                throw new InvalidOperationException("Book is already available.");
            Status = BookStatus.Available;
        }

        public void UpdateDetails(string title, string author, int publicationYear, string genre)
        {
            Title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
            Author = Guard.Against.NullOrWhiteSpace(author, nameof(author));
            PublicationYear = Guard.Against.NegativeOrZero(publicationYear, nameof(publicationYear));
            Genre = Guard.Against.NullOrWhiteSpace(genre, nameof(genre));
        }
    }
}