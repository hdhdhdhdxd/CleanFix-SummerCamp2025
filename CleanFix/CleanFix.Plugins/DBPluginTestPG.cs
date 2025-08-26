using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Logging;
using CleanFix.Plugins;

namespace CleanFix.Plugins
{
    /// <summary>
    /// Plugin de acceso a base de datos para CleanFixBot. Permite consultar empresas y materiales.
    /// </summary>
    public class DBPluginTestPG : IPlugin
    {
        private readonly string _connectionString;
        private readonly ILogger<DBPluginTestPG> _logger;

        /// <summary>
        /// Constructor del plugin. Recibe la cadena de conexión y un logger opcional.
        /// </summary>
        public DBPluginTestPG(string connectionString, ILogger<DBPluginTestPG> logger = null)
        {
            _connectionString = connectionString;
            _logger = logger;
            LogInfo($"[DBPluginTestPG] Cadena de conexión recibida: {_connectionString}");
        }

        // Métodos auxiliares para logging
        private void LogInfo(string message)
        {
            if (_logger != null)
                _logger.LogInformation(message);
            else
                Console.WriteLine(message);
        }
        private void LogError(string message)
        {
            if (_logger != null)
                _logger.LogError(message);
            else
                Console.WriteLine(message);
        }

        /// <summary>
        /// Obtiene todas las empresas de la base de datos y las mapea a CompanyIa (sin Id ni IssueTypeId).
        /// </summary>
        [KernelFunction]
        public EmpresaResponse GetAllEmpresas()
        {
            var companiesIa = new List<CompanyIa>();
            try
            {
                LogInfo("[DBPluginTestPG] Intentando abrir conexión para empresas...");
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                LogInfo("[DBPluginTestPG] Conexión abierta correctamente para empresas.");
                var command = new SqlCommand("SELECT Name, Address, Number, Email, Price, WorkTime FROM dbo.Companies", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var emp = new CompanyIa
                    {
                        Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Address = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Number = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Price = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4),
                        WorkTime = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                    };
                    LogInfo($"[DBPluginTestPG] Empresa obtenida: Name={emp.Name}, Price={emp.Price}");
                    companiesIa.Add(emp);
                }
            }
            catch (Exception ex)
            {
                LogError($"[DBPluginTestPG] Error al obtener empresas: {ex.Message}");
                return new EmpresaResponse { Success = false, Error = ex.Message };
            }
            LogInfo($"[DBPluginTestPG] Empresas obtenidas: {companiesIa.Count}");
            return new EmpresaResponse { Success = true, Data = companiesIa };
        }

        /// <summary>
        /// Obtiene todos los materiales de la base de datos y los mapea a MaterialIa (sin Id ni IssueTypeId).
        /// </summary>
        [KernelFunction]
        public MaterialResponse GetAllMaterials()
        {
            var materialesIa = new List<MaterialIa>();
            try
            {
                LogInfo("[DBPluginTestPG] Intentando abrir conexión para materiales...");
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                LogInfo("[DBPluginTestPG] Conexión abierta correctamente para materiales.");
                var command = new SqlCommand("SELECT Name, Cost FROM dbo.Materials", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var mat = new MaterialIa
                    {
                        Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                        Cost = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                        Available = true
                    };
                    LogInfo($"[DBPluginTestPG] Material obtenido: Name={mat.Name}, Cost={mat.Cost}");
                    materialesIa.Add(mat);
                }
            }
            catch (Exception ex)
            {
                LogError($"[DBPluginTestPG] Error al obtener materiales: {ex.Message}");
                return new MaterialResponse { Success = false, Error = ex.Message };
            }
            LogInfo($"[DBPluginTestPG] Materiales obtenidos: {materialesIa.Count}");
            return new MaterialResponse { Success = true, Data = materialesIa };
        }

        /// <summary>
        /// Procesa un mensaje del bot y ejecuta la consulta correspondiente sobre empresas o materiales.
        /// </summary>
        public async Task<PluginRespuesta> EjecutarAsync(string mensaje)
        {
            mensaje = mensaje.ToLower();
            LogInfo($"[DBPluginTestPG] EjecutarAsync llamado con mensaje: {mensaje}");

            // Procesamiento de empresas
            var empresasResponse = await Task.Run(() => GetAllEmpresas());
            var empresas = empresasResponse.Data as List<CompanyIa>;
            if (!empresasResponse.Success || empresas == null)
            {
                LogError($"[DBPluginTestPG] Error en empresas: {empresasResponse.Error}");
                return new PluginRespuesta { Success = false, Error = empresasResponse.Error ?? "Error al obtener empresas." };
            }

            // Filtros y patrones para empresas
            // ...puedes añadir aquí patrones si lo necesitas...

            if (mensaje.Contains("más barata"))
            {
                var empresaMasBarata = empresas.OrderBy(e => e.Price).FirstOrDefault();
                return new PluginRespuesta { Success = true, Data = empresaMasBarata != null ? new List<CompanyIa> { empresaMasBarata } : new List<CompanyIa>() };
            }

            if (mensaje.Contains("más cara"))
            {
                var empresaMasCara = empresas.OrderByDescending(e => e.Price).FirstOrDefault();
                return new PluginRespuesta { Success = true, Data = empresaMasCara != null ? new List<CompanyIa> { empresaMasCara } : new List<CompanyIa>() };
            }

            if (mensaje.StartsWith("empresas"))
                return new PluginRespuesta { Success = true, Data = empresas };

            // Procesamiento de materiales
            if (mensaje.StartsWith("materiales"))
            {
                var materialesResponse = await Task.Run(() => GetAllMaterials());
                var materiales = materialesResponse.Data as List<MaterialIa>;
                if (!materialesResponse.Success || materiales == null)
                {
                    LogError($"[DBPluginTestPG] Error en materiales: {materialesResponse.Error}");
                    return new PluginRespuesta { Success = false, Error = materialesResponse.Error ?? "Error al obtener materiales." };
                }

                // Uso de patrones y frases del parser
                if (ConsultaParser.SolicitaTodosMateriales(mensaje))
                    materiales = materiales.Where(m => m.Available).ToList();

                if (ConsultaParser.SolicitaMasBarato(mensaje))
                {
                    var materialMasBarato = materiales.OrderBy(m => m.Cost).FirstOrDefault();
                    return new PluginRespuesta { Success = true, Data = materialMasBarato != null ? new List<MaterialIa> { materialMasBarato } : new List<MaterialIa>() };
                }

                if (ConsultaParser.SolicitaMasCaro(mensaje))
                {
                    var materialMasCaro = materiales.OrderByDescending(m => m.Cost).FirstOrDefault();
                    return new PluginRespuesta { Success = true, Data = materialMasCaro != null ? new List<MaterialIa> { materialMasCaro } : new List<MaterialIa>() };
                }

                return new PluginRespuesta { Success = true, Data = materiales };
            }

            LogError("[DBPluginTestPG] Consulta no reconocida.");
            return new PluginRespuesta
            {
                Success = false,
                Error = "🤖 No entendí tu mensaje. Prueba con 'empresas tipo=2' o 'materiales costo<300'.",
                Data = null
            };
        }
    }

    /// <summary>
    /// Respuesta de consulta de empresas para el bot.
    /// </summary>
    public class EmpresaResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<CompanyIa> Data { get; set; }
    }

    /// <summary>
    /// Respuesta de consulta de materiales para el bot.
    /// </summary>
    public class MaterialResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public List<MaterialIa> Data { get; set; }
    }

    /// <summary>
    /// Modelo de empresa para el bot (sin Id ni IssueTypeId).
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
    /// Modelo de material para el bot (sin Id ni IssueTypeId).
    /// </summary>
    public class MaterialIa
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool Available { get; set; }
    }
}






