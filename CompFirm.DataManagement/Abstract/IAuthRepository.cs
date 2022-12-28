using CompFirm.Dto.Users;
using System.Threading.Tasks;

namespace CompFirm.DataManagement.Abstract
{
    public interface IAuthRepository
    {
        Task SignUp(CreateUserRequestDto userRequest);

        Task<AuthResultDto> SignIn(string login, string password);

        Task<PayloadDto> GetUserPayload(string token);

        Task<UserInfoDto> GetUserInfo(string token);

        Task UpdateUserInfo(UpdateUserInfoRequestDto fullUserInfo, string token);


    }
}
