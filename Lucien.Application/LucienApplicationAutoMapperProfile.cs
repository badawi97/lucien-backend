using AutoMapper;
using Lucien.Application.Contracts.Cards.Dto;
using Lucien.Application.Contracts.Sessions.Dto;
using Lucien.Application.Contracts.Users.Dtos;
using Lucien.Domain.Cards.Entities;
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

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();

            CreateMap<Session, SessionDto>().ReverseMap();
            CreateMap<Session, CreateSessionDto>().ReverseMap();
            CreateMap<Session, UpdateSessionDto>().ReverseMap();


        }
    }
}
