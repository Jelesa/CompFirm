using CompFirm.Dto.Manufacturers;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.Users;

namespace CompFirm.DataManagement.Abstract
{
    public interface IManufacturersRepository
    {
        Task<IReadOnlyCollection<ManufacturerDto>> GetManufacturers();

        Task CreateManufacturer(PayloadDto payload, CreateManufacturerRequestDto request);

        Task DeleteManufacturer(PayloadDto payload, int id);
    }
}
