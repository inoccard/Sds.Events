using System.Collections.Generic;

namespace Sds.Events.WebAPI.Dtos
{
    public class SpeakerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MiniCurriculum { get; set; }
        public string ImageUrl { get; set; }
        public string ContactCell { get; set; }
        public string ContactEmail { get; set; }
        public List<SocialNetworkDto> SocialNetworks { get; set; }
        public List<EventDto> Events { get; set; }
    }
}