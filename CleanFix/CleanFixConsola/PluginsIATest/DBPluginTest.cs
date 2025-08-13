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

            var command = new SqlCommand("SELECT Id, Name, Address, Number, Email, [type], Price, WorkTime FROM dbo.Companies", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                companies.Add(new Company
                {
                    Id = reader.GetGuid(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Address = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Number = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Type = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                    Price = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                    WorkTime = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                });
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }

        return JsonSerializer.Serialize(new { success = true, data = companies }, new JsonSerializerOptions { WriteIndented = true });
    }

    [KernelFunction, Description("Obtiene todos los materiales desde la base de datos, las convierte a JSON")]
    public string GetAllMaterials()
    {
        var materiales = new List<Material>();

        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = new SqlCommand("SELECT Id, Name, Cost, Issue FROM dbo.Materials", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                materiales.Add(new Material
                {
                    Id = reader.GetGuid(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Cost = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                    Issue = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    Available = true // Assuming materials are available by default
                });
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }

        return JsonSerializer.Serialize(new { success = true, data = materiales }, new JsonSerializerOptions { WriteIndented = true });
    }
}


public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }

    public string Email { get; set; }
    public int Type { get; set; }
    public decimal Price { get; set; }
    public int WorkTime { get; set; }
}
public class Material
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public double Cost { get; set; }
    public int Issue { get; set; }
    public bool Available { get; set; } = true; // Assuming materials are available by default
    public Guid SolicitationId { get; set; }
}



