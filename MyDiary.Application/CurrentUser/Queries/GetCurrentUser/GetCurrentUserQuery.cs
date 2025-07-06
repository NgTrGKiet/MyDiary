using MediatR;
using MyDiary.Application.CurrentUser.Dtos;

namespace MyDiary.Application.CurrentUser.Queries.GetCurrentUser;

public class GetCurrentUserQuery() : IRequest<UserDto>
{
    
}