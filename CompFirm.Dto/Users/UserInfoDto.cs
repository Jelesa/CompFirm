namespace CompFirm.Dto.Users
{
    public class UserInfoDto : PayloadDto
    {
        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public string Phone { get; set; }
    }
}
