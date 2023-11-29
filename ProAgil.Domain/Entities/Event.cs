using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProAgil.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Local { get; set; }
        public DateTime EventDate { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string Theme { get; set; }
        public int PersonQtd { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ImageURL { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ContactPhone { get; set; }
        [Column(TypeName = "varchar(500)")]
        public string ContactEmail { get; set; }

        public List<Lot> Lots { get; set; }
        public List<SocialNetWork> SocialNetworks { get; set; }
        public List<SpeakerEvent> SpeakerEvents { get; set; }

    }
}