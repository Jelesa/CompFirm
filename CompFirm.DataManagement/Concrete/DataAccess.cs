using CompFirm.DataManagement.Abstract;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace CompFirm.DataManagement.Concrete
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;

        public DataAccess(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<MySqlConnection> GetConnection()
        {
            var connectionString = this.configuration.GetValue<string>("ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Не указана настройка строки подключения");
            }

            var connection = new MySqlConnection(connectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
