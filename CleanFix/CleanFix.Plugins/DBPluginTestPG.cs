using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;

namespace CleanFix.Plugins
{
    public class DBPluginTestPG : IPlugin
    {
        private readonly string _connectionString;

        public DBPluginTestPG(string connectionString)
        {
            _connectionString = connectionString;
        }

        [KernelFunction, Description("Obtiene todas las empresas desde la base de datos, las convierte a objeto")]
        public EmpresaResponse GetAllEmpresas()
        {
            var companiesIa = new List<CompanyIa>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand("SELECT Id, Name, Address, Number, Email, [type], Price, WorkTime FROM dbo.Companies", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    companiesIa.Add(new CompanyIa
                    {
                        Id = reader.GetInt32(0),
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
                return new EmpresaResponse { Success = false, Error = ex.Message };
            }

            return new EmpresaResponse { Success = true, Data = companiesIa };
        }

        [KernelFunction, Description("Obtiene todos los materiales desde la base de datos, las convierte a objeto")]
        public MaterialResponse GetAllMaterials()
        {
            var materialesIa = new List<MaterialIa>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                var command = new SqlCommand("SELECT Id, Name, Cost, Issue FROM dbo.Materials", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    materialesIa.Add(new MaterialIa
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Cost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Issue = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Available = true
                    });
                }
            }
            catch (Exception ex)
            {
                return new MaterialResponse { Success = false, Error = ex.Message };
            }

            return new MaterialResponse { Success = true, Data = materialesIa };
        }

        // ✅ Implementación obligatoria de IPlugin
        public async Task<string> EjecutarAsync(string mensaje)
        {
            // Puedes usar Task.Run para envolver el método sincrónico
            var response = await Task.Run(() => GetAllEmpresas());

            if (!response.Success)
                return $"❌ Error al obtener empresas: {response.Error}";

            return JsonConvert.SerializeObject(response.Data, Formatting.Indented);
        }
    }

    // ✅ Clases auxiliares

    public class EmpresaResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<CompanyIa> Data { get; set; }
    }

    public class MaterialResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<MaterialIa> Data { get; set; }
    }

    public class CompanyIa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public int Type { get; set; }
        public decimal Price { get; set; }
        public int WorkTime { get; set; }
    }

    public class MaterialIa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Issue { get; set; }
        public bool Available { get; set; }
    }
}



