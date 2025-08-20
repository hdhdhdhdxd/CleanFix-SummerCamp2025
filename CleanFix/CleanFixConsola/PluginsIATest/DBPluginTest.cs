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

namespace CleanFixConsola.PluginsIATest
{
    public class DBPluginTest
    {
        // Almacena la cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la cadena de conexión y la guarda
        public DBPluginTest(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Función expuesta al kernel que obtiene todas las empresas desde la base de datos
        [KernelFunction, Description("Obtiene todas las empresas desde la base de datos, las convierte a objeto")]
        public object GetAllEmpresas()
        {
            var companies = new List<Company>();

            try
            {
                // Abre conexión con la base de datos
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                // Ejecuta consulta SQL para obtener los datos de las empresas
                var command = new SqlCommand("SELECT Id, Name, Address, Number, Email, [type], Price, WorkTime FROM dbo.Companies", connection);
                using var reader = command.ExecuteReader();

                // Recorre los resultados y los convierte en objetos Company
                while (reader.Read())
                {
                    companies.Add(new Company
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
                // En caso de error, devuelve una respuesta con el mensaje de excepción
                return new EmpresaResponse { Success = false, Error = ex.Message };
            }

            // Devuelve la lista de empresas con éxito
            return new EmpresaResponse { Success = true, Data = companies };
        }

        // Función expuesta al kernel que obtiene todos los materiales desde la base de datos
        [KernelFunction, Description("Obtiene todos los materiales desde la base de datos, las convierte a objeto")]
        public MaterialResponse GetAllMaterials()
        {
            var materiales = new List<Material>();

            try
            {
                // Abre conexión con la base de datos
                using var connection = new SqlConnection(_connectionString);
                connection.Open();

                // Ejecuta consulta SQL para obtener los datos de los materiales
                var command = new SqlCommand("SELECT Id, Name, Cost, Issue FROM dbo.Materials", connection);
                using var reader = command.ExecuteReader();

                // Recorre los resultados y los convierte en objetos Material
                while (reader.Read())
                {
                    materiales.Add(new Material
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Cost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        Issue = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Available = true // Se marca como disponible por defecto
                    });
                }
            }
            catch (Exception ex)
            {
                // En caso de error, devuelve una respuesta con el mensaje de excepción
                return new MaterialResponse
                {
                    Success = false,
                    Error = ex.Message
                };
            }

            // Devuelve la lista de materiales con éxito
            return new MaterialResponse
            {
                Success = true,
                Data = materiales
            };
        }
    }


    //Clases con los datos de respuesta

    public class Company
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
    public class Material
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal Cost { get; set; }

        public bool Available { get; set; } = true; // Assuming materials are available by default
        public int Issue { get; set; }
        public int SolicitationId { get; set; }
    }
}



