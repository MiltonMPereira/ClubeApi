using AutoMapper;
using ClubAccessControl.API.DTOs;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TentativaAcesso, TentativaAcessoDTO>()
            .ForMember(dest => dest.SocioNome, opt => opt.MapFrom(src => src.Socio != null ? src.Socio.Nome : string.Empty))
            .ForMember(dest => dest.AreaNome, opt => opt.MapFrom(src => src.AreaClube != null ? src.AreaClube.Nome : string.Empty));

            CreateMap<AreaDTO, AreaClube>();
            CreateMap<AreaClube, AreaDTO>()
                .ForMember(dest => dest.PlanosPermitidosIds, opt => opt.MapFrom(src => src.PlanosPermitidos.Select(p => p.Id).ToList()));

            CreateMap<PlanoDTO, PlanoAcesso>();
            CreateMap<PlanoAcesso, PlanoDTO>()
                .ForMember(dest => dest.AreasPermitidasIds, opt => opt.MapFrom(src => src.AreasPermitidas.Select(a => a.Id).ToList()));

            CreateMap<Socio, SocioDTO>()
                .ForMember(dest => dest.PlanoId, opt => opt.MapFrom(src => src.PlanoId));
            CreateMap<SocioDTO, Socio>();
            CreateMap<Socio, SocioReadDTO>()
                .ForMember(dest => dest.PlanoNome, opt => opt.MapFrom(src => src.Plano.Nome))
                .ForMember(dest => dest.AreasPermitidas, opt => opt.MapFrom(src => src.Plano.AreasPermitidas.Select(a => a.Nome).ToList()));
        }
    }
}
