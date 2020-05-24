using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProAgil.WebAPI.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        [Column (TypeName = "varchar(500)")]
        public string Local { get; set; }
        public DateTime EventDate { get; set; }
         [Column (TypeName = "varchar(500)")]
        public string Theme { get; set; }
        public int PersonQtd { get; set; }
         [Column (TypeName = "varchar(500)")]
        public string ImageURL { get; set; }
         [Column (TypeName = "varchar(500)")]
        public string ContactPhone { get; set; }
         [Column (TypeName = "varchar(500)")]
        public string contactEmail { get; set; }
        
        public List<LotDto> Lots { get; set; }
        public List<SocialNetworkDto> SocialNetworks { get; set; }
        public List<SpeakerDto> Speakers { get; set; }

    }
}