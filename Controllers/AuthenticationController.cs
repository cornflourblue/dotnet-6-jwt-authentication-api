namespace WebApi.Controllers;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using WebApi.Aplication.DTOs;
using WebApi.Helpers;
using WebApi.Infrascture.Command;
using WebApi.Infrascture.Querys;
using WebApi.Models;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator mediator;
 

    public AuthenticationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("api/token")]
    public async Task<ActionResult<UserResponseDto>> Authenticate(AuthenticateRequestCommand command)
    {
        var response = await this.mediator.Send(command);
        return Ok(response);
        //var response = _userService.Authenticate(command);
        //if (response == null)
        //    return BadRequest(new { message = "Username or password is incorrect" });

        //return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
    {
        var users = await this.mediator.Send(new GetAllUserQuery());
        return Ok(users);
    }
}
