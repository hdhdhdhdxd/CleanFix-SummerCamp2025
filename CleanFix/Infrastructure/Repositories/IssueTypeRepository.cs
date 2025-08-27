using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class IssueTypeRepository
    {
        private readonly string _connectionString;
        public IssueTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<IssueType>> GetAllAsync()
        {
            var result = new List<IssueType>();
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var command = new SqlCommand("SELECT Id, Name FROM IssueTypes", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(new IssueType
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            return result;
        }
    }
}
