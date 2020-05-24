using AutoMapper;
using ProAgil.Domain.Entities;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Event, EventDto>();
            CreateMap<Speaker, SpeakerDto>();
            CreateMap<Lot, LotDto>();
            CreateMap<SocialNetWork, SocialNetworkDto>();
        }
    }
}