using MediatR;
using System.Collections;
using System.Collections.Generic;
using WebApi.Aplication.DTOs;
using WebApi.Infrascture.Command;
using WebApi.Infrascture.Querys;
using WebApi.Services.Contracts;

namespace WebApi.Aplication.Handlers.Query
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, List<UserResponseDto>>
    {
        private readonly IAuthenticationJWTService authentacationService;

        public GetAllUserHandler(IAuthenticationJWTService authentacationService)
        {
            this.authentacationService = authentacationService;
        }

        public Task<List<UserResponseDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var response = this.authentacationService.GetAll();
            List<UserResponseDto> usersDto = response.Select(user => new UserResponseDto
            {
                Id = user.Iduser,
                Name = user.Name,
            }).ToList();

            return Task.FromResult(usersDto);
        }
    }
}
