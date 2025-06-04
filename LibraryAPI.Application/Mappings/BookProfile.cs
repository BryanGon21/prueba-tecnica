using AutoMapper;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Application.DTOs;

namespace LibraryAPI.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookRequest, Book>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.BookStatus.Available));
            CreateMap<UpdateBookRequest, Book>();
        }
    }
} 