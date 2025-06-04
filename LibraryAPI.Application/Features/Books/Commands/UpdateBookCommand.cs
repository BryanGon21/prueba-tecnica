using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using MediatR;

namespace LibraryAPI.Application.Features.Books.Commands
{
    public record UpdateBookCommand(Guid Id, UpdateBookRequest Request) : IRequest<bool>;

    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, bool>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null) return false;
            book.UpdateDetails(request.Request.Title, request.Request.Author, request.Request.PublicationYear, request.Request.Genre);
            await _repository.UpdateAsync(book, cancellationToken);
            return true;
        }
    }
}