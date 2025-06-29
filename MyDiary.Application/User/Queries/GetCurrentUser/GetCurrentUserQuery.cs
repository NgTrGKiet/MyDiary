using MediatR;
using MyDiary.Application.User.Dtos;

namespace MyDiary.Application.User.Queries.GetCurrentUser;

public class GetCurrentUserQuery() : IRequest<UserDto>
{
    
}