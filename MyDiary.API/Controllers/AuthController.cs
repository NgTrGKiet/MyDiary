using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MyDiary.Application.Auth.Models;
using MyDiary.Application.Contracts.Identity;
using MyDiary.Application.User.Dtos;
using MyDiary.Application.User.Queries.GetCurrentUser;

namespace MyDiary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    private readonly IMediator _mediator;

    public AuthController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request) {
        return Ok(await _authService.Login(request));
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        return Ok(await _authService.Register(request));
    }

    [HttpGet("GetCurrentUser")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        return Ok(await _mediator.Send(new GetCurrentUserQuery()));
    }
    
}