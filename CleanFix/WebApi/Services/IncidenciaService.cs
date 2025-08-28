using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class IncidenciaService : IIncidenciaService
    {
        public async Task ReportarIncidenciaAsync(string usuario, string descripcion, string tipo, string referencia)
        {
            // Guardar la incidencia en un archivo de texto (puedes cambiar a base de datos si lo prefieres)
            var incidencia = new
            {
                Usuario = usuario,
                Descripcion = descripcion,
                Tipo = tipo,
                Referencia = referencia,
                Fecha = DateTime.UtcNow
            };
            var json = JsonSerializer.Serialize(incidencia);
            await File.AppendAllTextAsync("incidencias.log", json + "\n");
        }
    }
}
