using System;
using System.Collections.Generic;

namespace ProAgil.Domain.Entities {
    public class Event {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime EventDate { get; set; }
        public string Theme { get; set; }
        public int PersonQtd { get; set; }
        public string ImageUrl { get; set; }
        public string ContactPhone { get; set; }
        public string contactEmail { get; set; }
        public List<Lot> Lots { get; set; }
        public List<SocialNetWork> SocialNetworks { get; set; }
        public List<SpeakerEvent> SpeakerEvents { get; set; }

    }
}