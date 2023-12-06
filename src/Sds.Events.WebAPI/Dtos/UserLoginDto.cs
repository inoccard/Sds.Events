namespace Sds.Events.WebAPI.Dtos
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
