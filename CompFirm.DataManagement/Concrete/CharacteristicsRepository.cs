using CompFirm.DataManagement.Abstract;
using CompFirm.Dto.Characteristics;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.DataManagement.Constants;
using CompFirm.Domain.Exceptions;
using CompFirm.Dto.Users;
using Dapper;

namespace CompFirm.DataManagement.Concrete
{
    public class CharacteristicsRepository : ICharacteristicsRepository
    {
        private readonly IDataAccess dataAccess;

        public CharacteristicsRepository(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<IReadOnlyCollection<CharacteristicDto>> GetCharacteristics()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                var chList = await connection.QueryAsync<CharacteristicDto>(QueryTexts.Characteristics.GetCharacteristics);

                return chList.AsList();
            }
        }

        public async Task Create(PayloadDto payload, CreateCharacteristicRequestDto request)
        {
            if (!payload.IsAdmin)
            {
                throw new BadRequestException("У вас нет прав на выполнение данной команды");
            }

            if (request == null || string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadRequestException("Не заполнено имя");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var checkExisting = await connection.QueryFirstOrDefaultAsync<CharacteristicDto>(
                    QueryTexts.Characteristics.GetCharacteristicByName,
                    new
                    {
                        name = request.Name
                    });

                if (checkExisting != null)
                {
                    throw new BadRequestException("Производитель с таким наименованием уже существует");
                }

                await connection.ExecuteAsync(
                    @"INSERT INTO `characteristics` (`name`, `unit`) VALUES (@name, @unit)",
                    new
                    {
                        name = request.Name,
                        unit = request.Unit ?? string.Empty
                    });
            }
        }

        public async Task Delete(PayloadDto payload, int id)
        {
            if (!payload.IsAdmin)
            {
                throw new BadRequestException("У вас нет прав на выполнение данной команды");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var checkExisting = await connection.QueryFirstOrDefaultAsync<int>(
                    QueryTexts.Characteristics.GetCountProducts,
                    new
                    {
                        id = id
                    });

                if (checkExisting > 0)
                {
                    throw new BadRequestException("Невозможно удалить характеристику, т.к. она указана в продуктах");
                }

                await connection.ExecuteAsync(
                    @"DELETE FROM `characteristics` WHERE Id=@id",
                    new
                    {
                        id = id
                    });
            }
        }
    }
}
