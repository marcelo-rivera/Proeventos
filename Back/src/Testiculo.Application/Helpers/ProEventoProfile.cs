using AutoMapper;
using Testiculo.Application.Dtos;
using Testiculo.Domain;
using Testiculo.Domain.Identity;
using Testiculo.Persistence.Models;

namespace testiculo.Helpers
{
    public class ProEventoProfile : Profile
    {
        public ProEventoProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();    
            CreateMap<Lote, LoteDto>().ReverseMap(); 
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap(); 
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();

            //CreateMap<PageList<Evento>, PageList<EventoDto>>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap(); 
            CreateMap<User, UserLoginDto>().ReverseMap(); 
            CreateMap<User, UserUpdateDto>().ReverseMap();              
        }
    }
}