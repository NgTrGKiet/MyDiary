using AutoMapper;
using MyDiary.Domain.Entities;

namespace MyDiary.Application.CurrentUser.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>().ReverseMap();
    }
}