using AutoMapper;
using NovelQT.Application.ViewModels;
using NovelQT.Domain.Models;

namespace NovelQT.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Customer, CustomerViewModel>();
        }
    }
}
