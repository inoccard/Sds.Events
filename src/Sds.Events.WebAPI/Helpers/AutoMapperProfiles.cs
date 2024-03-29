using AutoMapper;
using Sds.Events.Domain.Entities;
using Sds.Events.Domain.Identity;
using Sds.Events.WebAPI.Dtos;
using System.Linq;

namespace Sds.Events.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            /// <summary>
            /// Para eventos, trabalha-se com palestrantes | 
            /// Muitos para muitos
            /// </summary>
            /// <typeparam name="Event"></typeparam>
            /// <typeparam name="EventDto"></typeparam>
            /// <returns></returns>
            CreateMap<Event, EventDto>()
            .ForMember(d => d.Speakers, opt => // destinatário
            {
                opt.MapFrom(src => src.SpeakerEvents.Select(s => s.Spreaker).ToArray()); // origem
            }).ReverseMap();

            /// <summary>
            /// Para palestrante, trabalha-se com Eventos |
            /// Muitos para muitos
            /// </summary>
            /// <typeparam name="Speaker"></typeparam>
            /// <typeparam name="SpeakerDto"></typeparam>
            /// <returns></returns>
            CreateMap<Speaker, SpeakerDto>()
            .ForMember(dest => dest.Events, opt => // destinatário
            {
                opt.MapFrom(src => src.SpeakerEvents.Select(se => se.Event).ToArray()); // origem
            }).ReverseMap();

            CreateMap<Lot, LotDto>().ReverseMap();

            CreateMap<SocialNetWork, SocialNetworkDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}