using AutoMapper;
using WebApi.Aplication.DTOs;
using WebApi.Infrascture.Command;

namespace WebApi.Aplication.Handlers.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            {
                //CreateMap<AuthenticationResponse, AuthenticationResponseToken>()
                //    .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
            }
        }
    }
}
