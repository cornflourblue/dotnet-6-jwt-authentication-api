using MediatR;
using WebApi.Aplication.DTOs;

namespace WebApi.Infrascture.Querys
{
    public class GetAllUserQuery: IRequest<List<UserResponseDto>>
    {

    }
}
