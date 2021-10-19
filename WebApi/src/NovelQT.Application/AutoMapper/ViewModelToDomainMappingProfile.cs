using AutoMapper;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Commands;
using NovelQT.Domain.Commands.Author;
using NovelQT.Domain.Commands.Book;
using NovelQT.Domain.Commands.Category;

namespace NovelQT.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CustomerViewModel, RegisterNewCustomerCommand>()
                .ConstructUsing(c => new RegisterNewCustomerCommand(c.Name, c.Email, c.BirthDate));
            CreateMap<CustomerViewModel, UpdateCustomerCommand>()
                .ConstructUsing(c => new UpdateCustomerCommand(c.Id, c.Name, c.Email, c.BirthDate));

            CreateMap<AuthorViewModel, RegisterNewAuthorCommand>()
                .ConstructUsing(c => new RegisterNewAuthorCommand(c.Name));

            CreateMap<CategoryViewModel, RegisterNewCategoryCommand>()
                .ConstructUsing(c => new RegisterNewCategoryCommand(c.Name));

            CreateMap<BookViewModel, RegisterNewBookCommand>()
                .ConstructUsing(c => new RegisterNewBookCommand(c.Name, c.Key, c.Cover, c.Status, c.View, c.Like, c.AuthorId, c.CategoryId, c.IndexStatus));
            CreateMap<BookViewModel, UpdateBookCommand>()
                .ConstructUsing(c => new UpdateBookCommand(c.Id, c.Name, c.Key, c.Cover, c.Status, c.View, c.Like, c.AuthorId, c.CategoryId, c.IndexStatus));

            CreateMap<ChapterViewModel, RegisterNewChapterCommand>()
                .ConstructUsing(c => new RegisterNewChapterCommand(c.BookId, c.Order, c.Name, c.Url, c.Content, c.IndexStatus));

        }
    }
}
