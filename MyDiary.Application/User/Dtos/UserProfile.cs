using AutoMapper;
using MyDiary.Domain.Entities;

namespace MyDiary.Application.User.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>().ReverseMap();
    }
}