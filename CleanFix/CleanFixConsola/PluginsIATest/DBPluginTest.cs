using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using Microsoft.SemanticKernel;

namespace CleanFixConsola.PluginsIATest
{
    /// <summary>
    /// Plugin de acceso a base de datos para CleanFixBot en consola. Permite consultar empresas y materiales.
    /// </summary>
    public class DBPluginTest
    {
        // Almacena la cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la cadena de conexión y la guarda
        public DBPluginTest(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Función expuesta al kernel que obtiene todas las empresas desde la base de datos (sin Id ni Issue)
        [KernelFunction, Description("Obtiene todas las empresas desde la base de datos, las convierte a objeto")]
        public EmpresaResponse GetAllEmpresas()
        {
            var companiesIa = new List<CompanyIa>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var command = new SqlCommand("SELECT Name, Address, Number, Email, Price, WorkTime FROM dbo.Companies", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    companiesIa.Add(new CompanyIa
                    {
                        Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Address = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Number = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Price = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                        WorkTime = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                    });
                }
            }
            catch (Exception ex)
            {
                return new EmpresaResponse { Success = false, Error = ex.Message };
            }

            return new EmpresaResponse { Success = true, Data = companiesIa };
        }

        // Función expuesta al kernel que obtiene todos los materiales desde la base de datos (sin Id ni Issue)
        [KernelFunction, Description("Obtiene todos los materiales desde la base de datos, las convierte a objeto")]
        public MaterialResponse GetAllMaterials()
        {
            var materialesIa = new List<MaterialIa>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                var command = new SqlCommand("SELECT Name, Cost FROM dbo.Materials", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    materialesIa.Add(new MaterialIa
                    {
                        Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Cost = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                        Available = true
                    });
                }
            }
            catch (Exception ex)
            {
                return new MaterialResponse
                {
                    Success = false,
                    Error = ex.Message
                };
            }

            return new MaterialResponse
            {
                Success = true,
                Data = materialesIa
            };
        }
    }

    /// <summary>
    /// Modelo de empresa para el bot (sin Id ni Issue).
    /// </summary>
    public class CompanyIa
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public decimal Price { get; set; }
        public int WorkTime { get; set; }
    }

    /// <summary>
    /// Modelo de material para el bot (sin Id ni Issue).
    /// </summary>
    public class MaterialIa
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool Available { get; set; }
    }

    /// <summary>
    /// Respuesta de consulta de empresas para el bot.
    /// </summary>
    public class EmpresaResponse
    {
        public bool Success { get; set; }
        public List<CompanyIa> Data { get; set; }
        public string Error { get; set; }
    }

    /// <summary>
    /// Respuesta de consulta de materiales para el bot.
    /// </summary>
    public class MaterialResponse
    {
        public bool Success { get; set; }
        public List<MaterialIa> Data { get; set; }
        public string Error { get; set; }
    }
}



