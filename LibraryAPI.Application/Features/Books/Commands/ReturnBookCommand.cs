using LibraryAPI.Application.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Features.Books.Commands
{
    public record ReturnBookCommand(Guid Id) : IRequest<bool>;

    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, bool>
    {
        private readonly IBookRepository _repository;

        public ReturnBookCommandHandler(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null) return false;
            book.Return();
            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }
    }
}