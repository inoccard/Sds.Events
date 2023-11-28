using System.Collections.Generic;
using ProAgil.Domain.Identity;

namespace ProAgil.Domain.Entities
{
    public class Speaker
    {
        public int Id { get; set; }
        public string MiniCurriculum { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<SocialNetWork> SocialNetworks { get; set; }
        public List<SpeakerEvent> SpeakerEvents { get; set; }
    }
}