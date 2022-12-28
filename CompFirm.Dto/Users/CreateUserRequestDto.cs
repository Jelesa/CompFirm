namespace CompFirm.Dto.Users
{
    public class CreateUserRequestDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
    }
}
