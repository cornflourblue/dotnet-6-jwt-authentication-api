using MediatR;
using System.ComponentModel.DataAnnotations;
using WebApi.Aplication.DTOs;
using WebApi.Models;

namespace WebApi.Infrascture.Command
{
    public class  AuthenticateRequestCommand: IRequest<AuthenticationTokenDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
