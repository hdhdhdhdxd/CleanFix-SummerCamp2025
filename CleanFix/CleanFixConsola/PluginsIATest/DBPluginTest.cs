using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.SemanticKernel;

public class DBPluginTest
{
    private readonly string _connectionString;

    public DBPluginTest(string connectionString)
    {
        _connectionString = connectionString;
    }

    [KernelFunction, Description("Obtiene todas las empresas desde la base de datos, las convierte a JSON")]
    public string GetAllEmpresas()
    {
        var companies = new List<Company>();

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("SELECT Id, Name, Address, Number, [type], Price, WorkTime FROM dbo.Companies", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                companies.Add(new Company
                {
                    Id = reader.GetGuid(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Number = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Type = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                    Price = reader.IsDBNull(5) ? 0 : reader.GetDecimal(5),
                    WorkTime = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                });
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }

        return JsonSerializer.Serialize(new { success = true, data = companies }, new JsonSerializerOptions { WriteIndented = true });
    }
}

/*[KernelFunction, Description("Devuelve todos los materiales de la base de datos")]
public List<Materials> GetAllMateriales()
{
    var materials = new List<Materials>();
    using (var connection = new SqlConnection("Server=(localdb)\\\\mssqllocaldb;Database=CleanFixDB;Trusted_Connection=True;MultipleActiveResultSets=true\"\n  }"))
    {
        connection.Open();
        var command = new SqlCommand("SELECT * FROM Materials", connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            materials.Add(new Materials
            {

                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Cost = reader.GetFloat(2),
                Available = reader.GetBoolean(3),
                Issue = reader.GetInt32(4)
            });
        }
    }
    return materials;
}
}*/
public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
    public int Type { get; set; }
    public decimal Price { get; set; }
    public int WorkTime { get; set; }
}
public class Materials
{
    public int Id { get; set; }

    public string Name { get; set; }
    public float Cost { get; set; }
    public bool Available { get; set; }
    public int Issue { get; set; }
}



