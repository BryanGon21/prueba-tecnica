using LibraryAPI.Application.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Features.Books.Commands
{
    public record BorrowBookCommand(Guid Id) : IRequest<bool>;

    public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, bool>
    {
        private readonly IBookRepository _repository;

        public BorrowBookCommandHandler(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null) return false;
            book.Borrow();
            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }
    }
}