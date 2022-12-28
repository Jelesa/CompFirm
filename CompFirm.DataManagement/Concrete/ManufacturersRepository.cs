using CompFirm.DataManagement.Abstract;
using CompFirm.Dto.Manufacturers;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.DataManagement.Constants;
using CompFirm.Domain.Exceptions;
using CompFirm.Dto.Users;
using Dapper;

namespace CompFirm.DataManagement.Concrete
{
    public class ManufacturersRepository : IManufacturersRepository
    {
        private readonly IDataAccess dataAccess;

        public ManufacturersRepository(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<IReadOnlyCollection<ManufacturerDto>> GetManufacturers()
        {
            using (var connection = await this.dataAccess.GetConnection())
            {
                return (await connection.QueryAsync<ManufacturerDto>(QueryTexts.Manufacturers.GetManufacturers)).AsList();
            }
        }

        public async Task CreateManufacturer(PayloadDto payload, CreateManufacturerRequestDto request)
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
                var checkExisting = await connection.QueryFirstOrDefaultAsync<ManufacturerDto>(
                    QueryTexts.Manufacturers.GetManufacturerByName,
                    new
                    {
                        name = request.Name
                    });

                if (checkExisting != null)
                {
                    throw new BadRequestException("Производитель с таким наименованием уже существует");
                }

                await connection.ExecuteAsync(
                    @"INSERT INTO `manufacturers` (`name`) VALUES (@name)",
                    new
                    {
                        name = request.Name
                    });
            }
        }

        public async Task DeleteManufacturer(PayloadDto payload, int id)
        {
            if (!payload.IsAdmin)
            {
                throw new BadRequestException("У вас нет прав на выполнение данной команды");
            }

            using (var connection = await this.dataAccess.GetConnection())
            {
                var checkExisting = await connection.QueryFirstOrDefaultAsync<int>(
                    QueryTexts.Manufacturers.GetCountProducts,
                    new
                    {
                        id = id
                    });

                if (checkExisting > 0)
                {
                    throw new BadRequestException("Невозможно удалить производителя, т.к. он указан в продуктах");
                }

                await connection.ExecuteAsync(
                    @"DELETE FROM `manufacturers` WHERE Id=@id",
                    new
                    {
                        id = id
                    });
            }
        }
    }
}
