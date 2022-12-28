using System.Threading.Tasks;

namespace CompFirm.DataManagement.Abstract
{
    public interface IReportsRepository
    {
        Task<byte[]> GetPriceListPdf();

        Task<byte[]> GetPriceListCsv();

        Task<byte[]> GetOstatkiPdf();

        Task<byte[]> GetOstatkiCsv();
    }
}
