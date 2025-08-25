using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CleanFix.Plugins;
using Microsoft.SemanticKernel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CleanFix.Plugins
{
    public class DBPluginTestPG : IPlugin
    {
        private readonly string _connectionString;
        private readonly ILogger<DBPluginTestPG> _logger;

        // Permitir inyección de logger, pero mantener compatibilidad con consola
        public DBPluginTestPG(string connectionString, ILogger<DBPluginTestPG> logger = null)
        {
            _connectionString = connectionString;
            _logger = logger;
            LogInfo($"[DBPluginTestPG] Cadena de conexión recibida: {_connectionString}");
        }

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
                var command = new SqlCommand("SELECT Id, Name, Address, Number, Email, IssueTypeId, Price, WorkTime FROM dbo.Companies", connection);
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
                        IssueTypeId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        Price = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        WorkTime = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                    });
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
                var command = new SqlCommand("SELECT Id, Name, Cost, IssueTypeId FROM dbo.Materials", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    materialesIa.Add(new MaterialIa
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Cost = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2),
                        IssueTypeId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Available = true
                    });
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

        public async Task<PluginRespuesta> EjecutarAsync(string mensaje)
        {
            mensaje = mensaje.ToLower();
            LogInfo($"[DBPluginTestPG] EjecutarAsync llamado con mensaje: {mensaje}");

            var empresasResponse = await Task.Run(() => GetAllEmpresas());
            var empresas = empresasResponse.Data as List<CompanyIa>;
            if (!empresasResponse.Success || empresas == null)
            {
                LogError($"[DBPluginTestPG] Error en empresas: {empresasResponse.Error}");
                return new PluginRespuesta { Success = false, Error = empresasResponse.Error ?? "Error al obtener empresas." };
            }

            int? tipoEmpresa = ConsultaParser.ExtraerTipo(mensaje, "empresa");
            if (tipoEmpresa.HasValue)
                empresas = empresas.Where(e => e.IssueTypeId == tipoEmpresa.Value).ToList();

            if (mensaje.Contains("precio>"))
            {
                var precioStr = mensaje.Split("precio>")[1].Split(' ')[0];
                if (decimal.TryParse(precioStr, out decimal precio))
                    empresas = empresas.Where(e => e.Price > precio).ToList();
            }

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

            if (mensaje.StartsWith("materiales"))
            {
                var materialesResponse = await Task.Run(() => GetAllMaterials());
                var materiales = materialesResponse.Data as List<MaterialIa>;
                if (!materialesResponse.Success || materiales == null)
                {
                    LogError($"[DBPluginTestPG] Error en materiales: {materialesResponse.Error}");
                    return new PluginRespuesta { Success = false, Error = materialesResponse.Error ?? "Error al obtener materiales." };
                }

                int? tipoMaterial = ConsultaParser.ExtraerTipo(mensaje, "material");
                if (tipoMaterial.HasValue)
                    materiales = materiales.Where(m => m.IssueTypeId == tipoMaterial.Value).ToList();

                if (mensaje.Contains("costo<"))
                {
                    var costoStr = mensaje.Split("costo<")[1].Split(' ')[0];
                    if (decimal.TryParse(costoStr, out decimal costo))
                        materiales = materiales.Where(m => m.Cost < costo).ToList();
                }

                if (mensaje.Contains("disponibles"))
                    materiales = materiales.Where(m => m.Available).ToList();

                if (mensaje.Contains("más barato"))
                {
                    var materialMasBarato = materiales.OrderBy(m => m.Cost).FirstOrDefault();
                    return new PluginRespuesta { Success = true, Data = materialMasBarato != null ? new List<MaterialIa> { materialMasBarato } : new List<MaterialIa>() };
                }

                if (mensaje.Contains("más caro"))
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
        public int IssueTypeId { get; set; }
        public decimal Price { get; set; }
        public int WorkTime { get; set; }
    }

    public class MaterialIa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int IssueTypeId { get; set; }
        public bool Available { get; set; }
    }
}



