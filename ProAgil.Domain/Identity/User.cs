using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProAgil.Domain.Identity
{
    public class User :IdentityUser<int>
    {
        [Column(TypeName= "NVARCHAR(150)")]
        public string fulName { get; set; }
        public List<UserRole> UserRoles { get; set; }

    }
}