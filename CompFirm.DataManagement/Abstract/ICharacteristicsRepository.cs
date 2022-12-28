using System.Collections.Generic;
using System.Threading.Tasks;
using CompFirm.Dto.Characteristics;
using CompFirm.Dto.Users;

namespace CompFirm.DataManagement.Abstract
{
    public interface ICharacteristicsRepository
    {
        Task<IReadOnlyCollection<CharacteristicDto>> GetCharacteristics();

        Task Create(PayloadDto payload, CreateCharacteristicRequestDto request);

        Task Delete(PayloadDto payload, int id);
    }
}
