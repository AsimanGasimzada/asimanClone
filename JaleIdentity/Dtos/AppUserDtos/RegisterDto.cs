namespace JaleIdentity.Dtos.AppUserDtos
{
    public class RegisterDto
    {
        public string Fullname { get; set; } = null!;
        public string UserName { get; set; }=null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; }= null!;
    }
}
