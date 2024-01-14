using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sds.Events.WebAPI.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "varchar(50)")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O e-mail deve ter entre 5 e 50 caracteres")]
        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "O nome de usuário pode conter apenas letras, números e underscores.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "varchar(500)")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "O e-mail deve ter entre 5 e 500 caracteres")]
        [RegularExpression(@"^(?:(?!\buser@example\.com\b).)*$", ErrorMessage = "O endereço de e-mail deve ser diferente de 'user@example.com'")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "varchar(500)")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [Column(TypeName = "varchar(500)")]
        public string FullName { get; set; }
    }
}
