using CompFirm.DataManagement.Abstract;
using CompFirm.DataManagement.Constants;
using CompFirm.Domain.Exceptions;
using CompFirm.Domain.Models;
using CompFirm.Dto.Users;
using Dapper;
using Jose;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainConstants = CompFirm.Domain.Constants.Constants;

namespace CompFirm.DataManagement.Concrete
{
    public class AuthRepository : IAuthRepository
    {
        private const string DefaultRoleName = "User";

        private readonly IDataAccess dataAccess;
        private readonly string tokenSecretKey;

        public AuthRepository(
            IConfiguration config,
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
            this.tokenSecretKey = config.GetValue<string>(DomainConstants.TokenSecretConfigName);
        }

        public async Task<PayloadDto> GetUserPayload(string token)
        {
            var payload = GetPayload(token);

            using (var connection = await this.dataAccess.GetConnection())
            {
                var userRoles = (await connection.QueryAsync<UserRole>(
                    QueryTexts.Users.GetUserWithRoles,
                    new
                    {
                        login = payload.Login
                    })).AsList();

                if (userRoles == null || userRoles.Count == 0)
                {
                    throw new UnauthorizedException();
                }

                return new PayloadDto
                {
                    Login = userRoles.First().Login,
                    Roles = userRoles.Select(x => x.RoleName).ToArray()
                };
            }
        }

        public async Task<AuthResultDto> SignIn(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login)
                || string.IsNullOrWhiteSpace(password))
            {
                throw new BadRequestException("Не заполнены обязательные поля запроса.");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var userInfo = await connection.QueryFirstOrDefaultAsync<ShortUserInfo>(
                    QueryTexts.Users.GetUserInfo,
                    new
                    {
                        login = login,
                        password = password
                    });

                if (userInfo == null)
                {
                    throw new BadRequestException("Указан неверный логин/пароль.");
                }

                var token = JWT.Encode(userInfo, Encoding.Default.GetBytes(this.tokenSecretKey), JwsAlgorithm.HS256);

                return new AuthResultDto
                {
                    Token = token
                };
            }
        }

        public async Task SignUp(CreateUserRequestDto userRequest)
        {
            if (userRequest == null
                || string.IsNullOrWhiteSpace(userRequest.Login)
                || string.IsNullOrWhiteSpace(userRequest.Password)
                || string.IsNullOrWhiteSpace(userRequest.Phone))
            {
                throw new BadRequestException("Не заполнены обязательные поля запроса.");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var existingUser = await connection.QueryFirstOrDefaultAsync<int>(QueryTexts.Users.GetUserCount, userRequest);

                    if (existingUser > 0)
                    {
                        throw new BadRequestException("Пользователь с таким логином и/или телефоном уже существует.");
                    }

                    var userId = await connection.ExecuteAsync(QueryTexts.Users.CreateUser, userRequest);

                    await connection.ExecuteAsync(QueryTexts.Users.AddUserRole, new
                    {
                        userId = userId,
                        roleName = DefaultRoleName
                    });

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<UserInfoDto> GetUserInfo(string token)
        {
            var payload = GetPayload(token);

            using (var connection = await this.dataAccess.GetConnection())
            {
                var userRoles = (await connection.QueryAsync<UserInfo>(
                    QueryTexts.Users.GetUserWithRoles,
                    new
                    {
                        login = payload.Login
                    })).AsList();

                if (userRoles == null || userRoles.Count == 0)
                {
                    throw new UnauthorizedException();
                }

                return new UserInfoDto
                {
                    Login = userRoles.First().Login,
                    Surname = userRoles.First().Surname,
                    Name = userRoles.First().Name,
                    Patronymic = userRoles.First().Patronymic,
                    Phone = userRoles.First().Phone,
                    Roles = userRoles.Select(x => x.RoleName).ToArray()
                };
            }
        }

        public async Task UpdateUserInfo(UpdateUserInfoRequestDto requestDto, string token)
        {
            var payload = GetPayload(token);

            using (var connection = await this.dataAccess.GetConnection())
            {
                var user = await connection.QueryFirstOrDefaultAsync<string>(
                    QueryTexts.Users.GetUserInfo,
                    new
                    {
                        login = payload.Login,
                        password = requestDto.Password
                    });

                if (string.IsNullOrWhiteSpace(user))
                {
                    throw new BadRequestException("Указан неверный пароль");
                }

                await connection.ExecuteAsync(
                    QueryTexts.Users.UpdateUserInfo,
                    new
                    {
                        surname = requestDto.Surname,
                        name = requestDto.Name,
                        patronymic = requestDto.Patronymic,
                        login = payload.Login
                    });

                if (!string.IsNullOrWhiteSpace(requestDto.NewPassword))
                {
                    await connection.ExecuteAsync(
                        QueryTexts.Users.UpdatePassword,
                        new
                        {
                            login = payload.Login,
                            password = requestDto.NewPassword
                        });
                }
            }
        }

        private ShortUserInfo GetPayload(string token)
        {
            try
            {
                var clearToken = token.Replace("Bearer", string.Empty).Trim();

                return JWT.Decode<ShortUserInfo>(clearToken, Encoding.Default.GetBytes(this.tokenSecretKey));
            }
            catch (Exception ex)
            {
                throw new UnauthorizedException();
            }
        }
    }
}
