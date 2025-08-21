using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CleanFix.Plugins;
using Microsoft.SemanticKernel;

namespace CleanFix.Plugins
{
    public class DBPluginTestPG : IPlugin
    {
        private readonly string _connectionString;

        public DBPluginTestPG(string connectionString)
        {
            _connectionString = connectionString;
        }

        [KernelFunction]
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

        [KernelFunction]
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

        public async Task<PluginRespuesta> EjecutarAsync(string mensaje)
        {
            mensaje = mensaje.ToLower();

            if (mensaje.Contains("empresa"))
            {
                var empresas = new List<object>
                {
                    new { Id = 1, Nombre = "Empresa A" },
                    new { Id = 2, Nombre = "Empresa B" }
                };

                return await Task.FromResult(new PluginRespuesta
                {
                    Success = true,
                    Error = null,
                    Data = empresas
                });
            }

            if (mensaje.Contains("material"))
            {
                var materiales = new List<object>
                {
                    new { Id = 1, Nombre = "Material X" },
                    new { Id = 2, Nombre = "Material Y" }
                };

                return await Task.FromResult(new PluginRespuesta
                {
                    Success = true,
                    Error = null,
                    Data = materiales
                });
            }

            return await Task.FromResult(new PluginRespuesta
            {
                Success = false,
                Error = "🤖 No entendí tu mensaje. Prueba con 'empresas' o 'materiales'.",
                Data = null
            });
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



