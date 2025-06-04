using LibraryAPI.Application.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Features.Books.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest<bool>;

    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly IBookRepository _repository;

        public DeleteBookCommandHandler(IBookRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null) return false;
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return true;
        }
    }
}