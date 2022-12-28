namespace CompFirm.Dto.Users
{
    public class UpdateUserInfoRequestDto : UserInfoDto
    {
        public string Password { get; set; }

        public string NewPassword { get; set; }
    }
}
