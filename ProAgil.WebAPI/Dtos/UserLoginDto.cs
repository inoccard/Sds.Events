using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proagil.WebAPI.Dtos
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
