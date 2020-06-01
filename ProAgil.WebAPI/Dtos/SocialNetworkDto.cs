using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class SocialNetworkDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage="O campo {0} é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage="O campo {0} é obrigatório")]
        public string Url { get; set; }
    }
}