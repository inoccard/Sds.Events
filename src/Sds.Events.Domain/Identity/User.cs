using Microsoft.AspNetCore.Identity;
using Sds.Events.Domain.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sds.Events.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        [Column(TypeName = "NVARCHAR(150)")]
        public string FullName { get; set; }

        [Column(TypeName = "INT")]
        public Title Title { get; set; }

        [Column(TypeName = "INT")]
        public Function Function { get; set; }

        [Column(TypeName = "NVARCHAR(150)")]
        public string Description { get; set; }

        [Column(TypeName = "NVARCHAR(150)")]
        public string ImageUrl { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}