﻿namespace WebApi.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Aplication.DTOs;
using WebApi.Middkeware;
using WebApi.Infrascture.Command;
using WebApi.Infrascture.Querys;
using WebApi.Infrastructure.Command;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator mediator;
 

    public AuthenticationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("api")]
    public async Task<ActionResult<UserResponseDto>> Authenticate(AuthenticateRequestCommand command)
    {
        var response = await this.mediator.Send(command);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("api/user")]
    public async Task<ActionResult<int>> Crete(AuthenticateRequestCommand command)
    {
        var userId = await this.mediator.Send(new CreateUserCommand() { Username = command.Username, Password = command.Password });
        return Ok(userId);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
        var users = await this.mediator.Send(new GetAllUserQuery());
        return Ok(users);
    }
    
}
