using AutoMapper;
using NovelQT.Application.Responses;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Models;

namespace NovelQT.Application.AutoMapper
{
    public class DomainToResponseMappingProfile : Profile
    {
        public DomainToResponseMappingProfile()
        {
            CreateMap<Book, BookResponse>()
                .ConstructUsing(c => new BookResponse(c.Id, c.Name, c.Key, c.Cover, c.Status, c.View, c.Like, c.Author.Name, c.Category.Name));

        }
    }
}
