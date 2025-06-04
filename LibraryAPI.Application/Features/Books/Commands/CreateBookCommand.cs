using AutoMapper;
using LibraryAPI.Application.DTOs;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Domain.Entities;
using MediatR;

namespace LibraryAPI.Application.Features.Books.Commands
{
    public record CreateBookCommand(CreateBookRequest Request) : IRequest<BookDto>;

    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request.Request);
            await _repository.AddAsync(book, cancellationToken);
            return _mapper.Map<BookDto>(book);
        }
    }
}