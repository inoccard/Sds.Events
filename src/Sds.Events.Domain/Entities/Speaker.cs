using Sds.Events.Domain.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sds.Events.Domain.Entities
{
    public class Speaker
    {
        public int Id { get; set; }
        public string MiniCurriculum { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public List<SocialNetWork> SocialNetworks { get; set; }
        public List<SpeakerEvent> SpeakerEvents { get; set; }
    }
}