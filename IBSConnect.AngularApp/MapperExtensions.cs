using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using IBSConnect.Business.Models;
using IBSConnect.Data.Models;

namespace IBSConnect.AngularApp;

public static class MapperExtensions
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserListViewModel>();
        }
    }

    public static IServiceCollection AddMappers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        return serviceCollection;
    }
}