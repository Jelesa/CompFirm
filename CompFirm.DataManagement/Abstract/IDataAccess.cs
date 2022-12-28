using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace CompFirm.DataManagement.Abstract
{
    public interface IDataAccess
    {
        Task<MySqlConnection> GetConnection();
    }
}
