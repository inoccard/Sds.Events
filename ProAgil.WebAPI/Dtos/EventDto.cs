using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProAgil.WebAPI.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Campo obrigatório")]
        [Column (TypeName = "varchar(500)")]
        public string Local { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Column (TypeName = "varchar(500)")]
        [RequiredAttribute(ErrorMessage="O tema deve ser Preenchido")]
        public string Theme { get; set; }

        [Range(2, 12000, ErrorMessage="Quantidade de pessoas deve ser entre 2 a 12000")]
        public int PersonQtd { get; set; }

         [Column (TypeName = "varchar(500)")]
        public string ImageURL { get; set; }

        [Phone]
        [Column (TypeName = "varchar(500)")]
        public string ContactPhone { get; set; }
        [Column (TypeName = "varchar(500)")]
        public string contactEmail { get; set; }
        
        public List<LotDto> Lots { get; set; }
        public List<SocialNetworkDto> SocialNetworks { get; set; }
        public List<SpeakerDto> Speakers { get; set; }

    }
}