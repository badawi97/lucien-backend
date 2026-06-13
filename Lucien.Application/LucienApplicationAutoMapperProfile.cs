using AutoMapper;
using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Permissions.Dtos;
using Lucien.Application.Contracts.Roles.Dtos;
using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Domain.Cards.Entities;
using Lucien.Domain.Roles.Entites;
using Lucien.Domain.Sessions.Entities;
using Lucien.Domain.Users.Entities;

namespace Lucien.Application
{
    public class LucienApplicationAutoMapperProfile : Profile
    {
        public LucienApplicationAutoMapperProfile()
        {
            CreateMap<Card, CardDto>().ReverseMap();
            CreateMap<Card, CreateCardDto>().ReverseMap();
            CreateMap<Card, UpdateCardDto>().ReverseMap();

            CreateMap<User, UserDto>();
            CreateMap<User, CreateUserDto>().ReverseMap()
                .AfterMap((source, destination) =>
                {
                    if (source.RoleId.HasValue)
                    {
                        destination.AssignRole(source.RoleId.Value);
                    }
                });
            CreateMap<User, UpdateUserDto>().ReverseMap()
                .AfterMap((source, destination) =>
                {
                    if (source.RoleId.HasValue)
                    {
                        destination.AssignRole(source.RoleId.Value);
                    }
                });

            CreateMap<Role, RoleDto>();
            CreateMap<Permission, PermissionDto>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.Name.Value))
                .ForMember(destination => destination.RoleId, options => options.Ignore());

            CreateMap<Session, SessionDto>().ReverseMap();
            CreateMap<Session, CreateSessionDto>().ReverseMap();
            CreateMap<Session, UpdateSessionDto>().ReverseMap();


        }
    }
}
