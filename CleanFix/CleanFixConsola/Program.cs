using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;


/*using Dominio.Maintenance;

string titulo = "CleanFix";
int anchoConsola = Console.WindowWidth;
int posicionX1 = (anchoConsola - titulo.Length) / 2;

Console.Clear();

// Animación: mostrar letra por letra con color
Console.ForegroundColor = ConsoleColor.Cyan;
Console.SetCursorPosition(posicionX1, Console.CursorTop);
foreach (char c in titulo)
{
    Console.Write(c);
    Thread.Sleep(150); // Pausa para animación
}
Console.ResetColor();

// Línea en blanco después del título
Console.WriteLine("\n");

var company = new Company();

/*
 
- Nombre de la aplicación: Dificultad I()

   [-] Como [Usuario] quiero [var el nombre de la aplicacion] para [poder redonocer la aplicacion que estoy usando]

     ///// Criterio de Aceptación:
        - En la pantalla debe aparecer el nomre de la aplicación "CleanFix"
        - Debe aparecer el nombre situado arriba a la izquierda semicentrado

- Concepto de diseño/Interfaz (Angular): Dificultad IIIII

   [-] Como [Usuario] quiero [ver diseño de la aplicación] para [poder usar la aplicación]
    ///// Criterio de Aceptación:
          - Debe tener un estilo similar al mockup de la aplicación
          - Debe mostrar el logo y nombre de la aplicación
          - Debe mostrar un botón que daría acceso a un formulario de busqueda de empresas
          - Debe mostrar una lista de empresas con sus datos básicos (nombre, dirección, teléfono, email)

- Lógica de funcionamiento / Simulacro con grupo de empresas pequeño (5/10 empresas): Dificultad III

   [-] Como [Usuario] quiero [poder gestionar las empresas de mantenimiento] para [poder realizar tareas de mantenimiento]
///// Criterio de Aceptación:
          - Debe haber una clase empresa con propiedades basicas (nombre, dirección, teléfono, email, tipo, coste, tiempo de trabajo)
          - Debe haber constructores acordes a cada una de las empresas
          - Debe ser posible introducir un tipo de problema y mostrar una lista de empresas que pueden solucionar ese problema
 */

// crear empresas
/*
Empresa Empresa1 = new Empresa()
{
    Id = 1,
    Nombre = "Electricidad y Más",
    Direccion = "Calle de la Luz, 123",
    Telefono = "123-456-7890",
    Email = "ElectricidadyMás@gmail.com",
    Tipo = "Electrical",
    Coste = 50.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 0, 0) // 2 horas de trabajo
};

Empresa Empresa2 = new Empresa()
{
    Id = 2,
    Nombre = "Fontanería Rápida",
    Direccion = "Avenida del Agua, 456",
    Telefono = "987-654-3210",
    Email = "FontaneríaRápida@gmail.com",
    Tipo = "Plumbing",
    Coste = 40.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(1, 30, 0) // 1 hora y 30 minutos de trabajo
};

Empresa Empresa3 = new Empresa()
{
    Id = 3,
    Nombre = "Carpenterizacion Total",
    Direccion = "Plaza del Clima, 789",
    Telefono = "555-123-4567",
    Email = "CarpenterizacionTotal@gmail.com",
    Tipo = "Carpentry",
    Coste = 60.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(3, 0, 0) // 3 horas de trabajo

};

Empresa Empresa4 = new Empresa()
{
    Id= 4,
    Nombre = "Pinturas y Más",
    Direccion = "Calle del Color, 321",
    Telefono = "444-555-6666",
    Email = "PinturasyMás@gmail.com",
    Tipo = "Painting",
    Coste = 45.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 30, 0) // 2 horas y 30 minutos de trabajo
};

Empresa Empresa5 = new Empresa()
{
    Id = 5,
    Nombre = "Suelo Perfecto",
    Direccion = "Avenida del Suelo, 654",
    Telefono = "333-222-1111",
    Email = "SueloPerfecto@gmail.com",
    Tipo = "Flooring",
    Coste = 55.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(4, 0, 0) // 4 horas de trabajo

};
Empresa Empresa6 = new Empresa()
{
    Id = 6,
    Nombre = "Limpieza Total",
    Direccion = "Calle de la Limpieza, 987",
    Telefono = "222-333-4444",
    Email = "LimpiezaTotal@gmail.com",
    Tipo = "Cleaning",
    Coste = 30.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(1, 0, 0) // 1 hora de trabajo
};

Empresa Empresa7 = new Empresa()
{
    Id = 7,
    Nombre = "Fontanería Bros",
    Direccion = "Calle de las Reparaciones, 159",
    Telefono = "111-222-3333",
    Email = "ReparacionesGenerales@gmail.com",
    Tipo = "Ready",
    Coste = 70.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(5, 0, 0) // 5 horas de trabajo
};

Empresa Empresa8 = new Empresa()
{
    Id=8,
    Nombre = "Electricidad Express",
    Direccion = "Calle de la Electricidad, 852",
    Telefono = "888-777-6666",
    Email = "ElectricidadExpress@gmail.com",
    Tipo = "Electrical",
    Coste = 65.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(2, 15, 0) // 2 horas y 15 minutos de trabajo
};

Empresa Empresa9 = new Empresa()
{
    Id = 9,
    Nombre = "Carpintería Creativa",
    Direccion = "Calle de la Madera, 753",
    Telefono = "999-888-7777",
    Email = "CarpinteríaCreativa@gmail.com",
    Tipo = "Carpentry",
    Coste = 75.00m, // Coste por hora
    TiempoTrabajo = new TimeSpan(3, 30, 0) // 3 horas y 30 minutos de trabajo
};

// Añadimos todas las empresas a una lista para facilitar su gestión
List<Empresa> Empresas = new List<Empresa>
    {
        Empresa1,
        Empresa2,
        Empresa3,
        Empresa4,
        Empresa5,
        Empresa6,
        Empresa7,
        Empresa8,
        Empresa9
    };

// Método para buscar empresas por tipo de problema

Console.WriteLine("Ingrese el número del tipo de problema: (introduzca 's' Para Salir) ");
Console.WriteLine();

// Mostrar título animado (puedes reutilizar el código anterior aquí)

// Colores para los issues
ConsoleColor[] colores = {
            ConsoleColor.Yellow,
            ConsoleColor.Green,
            ConsoleColor.Magenta,
            ConsoleColor.Red
        };

foreach (IssueType estado in Enum.GetValues(typeof(IssueType)))
{
    if (estado == IssueType.Ready)
    {
        continue; // Saltar el estado "Ready"
    }

    string estadoTexto = estado.ToString();
    int posicionX2 = (anchoConsola - estadoTexto.Length) / 10;

    Console.ForegroundColor = colores[(int)estado % colores.Length];
    Console.SetCursorPosition(posicionX2, Console.CursorTop);
    Console.WriteLine($"{estadoTexto}: {(int)estado}");
    Console.ResetColor();

    // Separación visual entre issues
    Console.WriteLine();
    Thread.Sleep(200); // Animación opcional
}

var respuesta = Console.ReadLine();



while (respuesta != "s")
{
    switch (respuesta)
    {
        case "0":
            Console.WriteLine("Empresas de Electricidad:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Electrical"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "1":
            Console.WriteLine("Empresas de Fontanería:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Plumbing"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "2":
            Console.WriteLine("Empresas de Carpintería:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Carpentry"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "3":
            Console.WriteLine("Empresas de Pintura:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Painting"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "4":
            Console.WriteLine("Empresas de Suelos:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Flooring"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        case "5":
            Console.WriteLine("Empresas de Limpieza:");
            foreach (var empresa in Empresas.Where(e => e.Tipo == "Cleaning"))
            {
                MostrarRespuesta(empresa.MostrarInformacion(), ConsoleColor.Yellow);
            }
            break;
        default:
            MostrarRespuesta("Tipo de problema no válido. Por favor, ingrese un número válido.(introduzca 's' Para Salir)", ConsoleColor.Red);
            break;
    }

    Console.WriteLine("Ingrese el número del tipo de problema: (introduzca 's' Para Salir) ");
    respuesta = Console.ReadLine();

}

MostrarRespuesta("Gracias por usar CleanFix. ¡Hasta luego!", ConsoleColor.Cyan);
Thread.Sleep(1000);
Environment.Exit(0);

static void MostrarRespuesta(string mensaje, ConsoleColor color)
{
    Console.WriteLine(); // Línea en blanco antes del mensaje
    int anchoConsola = Console.WindowWidth;
    int posicionX = (anchoConsola - mensaje.Length) / 2;
    Console.ForegroundColor = color;

    foreach (char c in mensaje)
    {
        Console.Write(c);
        Thread.Sleep(5); // Animación letra por letra
    }
    Console.ResetColor();
    Console.WriteLine("\n");
}

*/


/*class Program
{
    static async Task Main(string[] args)
    {
        // 1. Crear el builder y configurar Azure OpenAI
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4.1",
            endpoint: "https://hdhdh-mdx2smel-eastus2.cognitiveservices.azure.com/",
            apiKey: "9ZMpVj9cCWRyv73s8vyxd0RL93ELHrtmNwN68ZPxRlDgBDjEgxR0JQQJ99BHACHYHv6XJ3w3AAAAACOGEv9e"
        );

        // 2. Agregar plugins de base de datos y factura
        string connectionString = "Server=tcp:devdemoserverbbdd.database.windows.net,1433;Initial Catalog=devdemobbdd2;Persist Security Info=False;User ID=admsql;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        var dbPlugin = new DBPluginTest(connectionString);
        builder.Plugins.AddFromObject(dbPlugin, "DBPlugin");

        var facturaPlugin = new FacturaPluginTest();
        builder.Plugins.AddFromObject(facturaPlugin, "FacturaPlugin");

        // 3. Construir el kernel
        var kernel = builder.Build();

        // 4. Obtener datos de empresas
        var empresasIA = await kernel.InvokeAsync("DBPlugin", "GetAllEmpresas");
        var empresasJson = empresasIA.GetValue<string>();

        if (string.IsNullOrWhiteSpace(empresasJson) || empresasJson.Contains("error"))
        {
            Console.WriteLine("❌ Error al obtener las empresas:");
            Console.WriteLine(empresasJson);
            return;
        }

        var companies = JsonSerializer.Deserialize<List<Company>>(empresasJson);

        // 5. Obtener datos de materiales
        var materialesIA = await kernel.InvokeAsync("DBPlugin", "GetAllMaterials");
        var materialesJson = materialesIA.GetValue<string>();

        if (string.IsNullOrWhiteSpace(materialesJson) || materialesJson.Contains("error"))
        {
            Console.WriteLine("❌ Error al obtener los materiales:");
            Console.WriteLine(materialesJson);
            return;
        }

        var materials = JsonSerializer.Deserialize<List<Material>>(materialesJson);

        // 6. Prompt general para preguntas
        var promptTemplate = @"
Tienes la siguiente información de empresas (companies) en JSON:
{{$empresas}}

Y también la siguiente información de materiales (materials) en JSON:
{{$materiales}}

Usa esta información para responder la pregunta del usuario:
{{$pregunta}}

Si no tiene relación, responde que no puedes ayudar.
";

        var promptFunction = kernel.CreateFunctionFromPrompt(promptTemplate);

        // 7. Inicio de interacción
        Console.WriteLine("¡Hola! Soy CleanFixBot. ¿En qué puedo ayudarte? Escribe 'factura' para generar una.");

        while (true)
        {
            Console.Write("> ");
            string userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(userInput)) continue;
            if (userInput.Equals("salir", StringComparison.OrdinalIgnoreCase)) break;

            // Comando especial para generar factura
            if (userInput.Contains("factura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("🧾 Generando factura...");

                var empresa = companies.FirstOrDefault();
                var materialesDisponibles = materials.Where(m => m.Available).ToList();

                if (empresa == null || materialesDisponibles.Count == 0)
                {
                    Console.WriteLine("❌ No hay empresa o materiales disponibles para generar la factura.");
                    continue;
                }

                var resultFactura = await kernel.InvokeAsync("FacturaPlugin", "GenerarFactura", new()
                {
                    ["materiales"] = materialesDisponibles,
                    ["empresa"] = empresa
                });

                var facturaJson = resultFactura.GetValue<string>();
                Console.WriteLine($"\n📄 Factura generada:\n{facturaJson}\n");

                continue;
            }

            // 8. Ejecutar prompt para otras preguntas
            var kernelArgs = new KernelArguments
            {
                ["empresas"] = empresasJson,
                ["materiales"] = materialesJson,
                ["pregunta"] = userInput
            };

            var responseData = await promptFunction.InvokeAsync(kernel, kernelArgs);
            Console.WriteLine($"\n🧠 CleanFixBot: {responseData.GetValue<string>()}\n");
        }
    }
}

// Define la clase genérica para la respuesta API
/*public class ApiResponse<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public List<T> Data { get; set; }
}
*/
// Define tus modelos (Company y Material)


// Aquí asumimos que FacturaPluginTest y DBPluginTest están definidos y funcionan correctamente

/*class Program
{
    static async Task Main(string[] args)
    {
        // 1. Crear el builder y configurar Azure OpenAI
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4.1",
            endpoint: "https://hdhdh-mdx2smel-eastus2.cognitiveservices.azure.com/",
            apiKey: "9ZMpVj9cCWRyv73s8vyxd0RL93ELHrtmNwN68ZPxRlDgBDjEgxR0JQQJ99BHACHYHv6XJ3w3AAAAACOGEv9e"
        );

        // 2. Agregar plugins
        string connectionString = "Server=tcp:devdemoserverbbdd.database.windows.net,1433;Initial Catalog=devdemobbdd2;Persist Security Info=False;User ID=admsql;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        var dbPlugin = new DBPluginTest(connectionString);
        builder.Plugins.AddFromObject(dbPlugin, "DBPlugin");

        var facturaPlugin = new FacturaPluginTest();
        builder.Plugins.AddFromObject(facturaPlugin, "FacturaPlugin");

        // 3. Construir kernel
        var kernel = builder.Build();

        // 4. Obtener empresas y materiales (sin mostrar JSON)
        var empresasIA = await kernel.InvokeAsync("DBPlugin", "GetAllEmpresas");
        var empresasJson = empresasIA.GetValue<string>();

        if (string.IsNullOrWhiteSpace(empresasJson) || empresasJson.Contains("error"))
        {
            Console.WriteLine("❌ Error al obtener las empresas:");
            return;
        }

        var responseEmpresas = JsonSerializer.Deserialize<ApiResponse<Company>>(empresasJson);
        var companies = responseEmpresas?.Data ?? new List<Company>();

        var materialesIA = await kernel.InvokeAsync("DBPlugin", "GetAllMaterials");
        var materialesJson = materialesIA.GetValue<string>();

        if (string.IsNullOrWhiteSpace(materialesJson) || materialesJson.Contains("error"))
        {
            Console.WriteLine("❌ Error al obtener los materiales:");
            return;
        }

        var responseMateriales = JsonSerializer.Deserialize<ApiResponse<Material>>(materialesJson);
        var materials = responseMateriales?.Data ?? new List<Material>();

        // 5. Prompt base para preguntas
        var promptTemplate = @"
Tienes la siguiente información de empresas (companies) en JSON:
{{$empresas}}

Y también la siguiente información de materiales (materials) en JSON:
{{$materiales}}

Usa esta información para responder la pregunta del usuario:
{{$pregunta}}

Si no tiene relación, responde que no puedes ayudar.
";

        var promptFunction = kernel.CreateFunctionFromPrompt(promptTemplate);

        Console.WriteLine("¡Hola! Soy CleanFixBot. Para generar una factura escribe 'factura' y sigue las indicaciones. Escribe 'salir' para terminar.");

        while (true)
        {
            Console.Write("> ");
            string userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(userInput)) continue;
            if (userInput.Equals("salir", StringComparison.OrdinalIgnoreCase)) break;

            if (userInput.StartsWith("factura", StringComparison.OrdinalIgnoreCase))
            {
                // Aquí pides datos específicos para la factura
                Console.WriteLine("Introduce el ID de la empresa:");
                string empresaIdInput = Console.ReadLine()?.Trim();
                var empresa = companies.FirstOrDefault(c => c.Id.ToString() == empresaIdInput);
                if (empresa == null)
                {
                    Console.WriteLine("❌ Empresa no encontrada.");
                    continue;
                }

                var selectedMaterials = new List<Material>();
                while (true)
                {
                    Console.WriteLine("Introduce el ID del material a añadir (o escribe 'fin' para terminar):");
                    string matInput = Console.ReadLine()?.Trim();
                    if (matInput.Equals("fin", StringComparison.OrdinalIgnoreCase)) break;

                    if (int.TryParse(matInput, out int matId))
                    {
                        var material = materials.FirstOrDefault(m => m.Id == matId);
                        if (material != null)
                        {
                            selectedMaterials.Add(material);
                            Console.WriteLine($"Material '{material.Name}' añadido.");
                        }
                        else
                        {
                            Console.WriteLine("Material no encontrado.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID inválido.");
                    }
                }

                if (selectedMaterials.Count == 0)
                {
                    Console.WriteLine("❌ No se seleccionaron materiales para la factura.");
                    continue;
                }

                // Invocar plugin para generar factura con los datos seleccionados
                // Asumiendo que ya tienes 'empresa' y 'selectedMaterials' definidos...

                // Para generar la factura completa
                var resultadoFactura = await kernel.InvokeAsync("FacturaPlugin", "GenerarFactura", new()
                {
                    ["empresa"] = empresa,
                    ["materiales"] = selectedMaterials
                });
                Console.WriteLine(resultadoFactura.GetValue<string>());

                // Para obtener solo el IVA total
                var resultadoIVA = await kernel.InvokeAsync("FacturaPlugin", "ObtenerIVA", new()
                {
                    ["empresa"] = empresa,
                    ["materiales"] = selectedMaterials
                });
                Console.WriteLine(resultadoIVA.GetValue<string>());
                continue;
            }

            // Respuesta estándar del bot con prompt
            var kernelArgs = new KernelArguments
            {
                ["empresas"] = empresasJson,
                ["materiales"] = materialesJson,
                ["pregunta"] = userInput
            };

            var responseData = await promptFunction.InvokeAsync(kernel, kernelArgs);
            Console.WriteLine($"\n🧠 CleanFixBot: {responseData.GetValue<string>()}\n");
        }
    }
}*/

// Clases auxiliares para deserialización
public class EmpresaResponse
{
    public bool Success { get; set; }
    public List<Company> Data { get; set; }
    public string Error { get; set; }
}

public class MaterialResponse
{
    public bool Success { get; set; }
    public List<Material> Data { get; set; }
    public string Error { get; set; }
}

/*class Program
{
    static async Task Main(string[] args)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4.1",
            endpoint: "https://hdhdh-mdx2smel-eastus2.cognitiveservices.azure.com/",
            apiKey: "9ZMpVj9cCWRyv73s8vyxd0RL93ELHrtmNwN68ZPxRlDgBDjEgxR0JQQJ99BHACHYHv6XJ3w3AAAAACOGEv9e"
        );

        builder.Services.AddLogging(cfg =>
        {
            cfg.AddConsole();
            cfg.SetMinimumLevel(LogLevel.Warning);
        });

        var dbPlugin = new DBPluginTest("Server=tcp:devdemoserverbbdd.database.windows.net,1433;Initial Catalog=devdemobbdd2;Persist Security Info=False;User ID=admsql;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        builder.Plugins.AddFromObject(dbPlugin, "DBPlugin");

        var facturaPlugin = new FacturaPluginTest();
        builder.Plugins.AddFromObject(facturaPlugin, "FacturaPlugin");

        var kernel = builder.Build();

        // Obtener empresas
        var empResult = await kernel.InvokeAsync("DBPlugin", "GetAllEmpresas");
        var empResponse = empResult.GetValue<EmpresaResponse>();

        if (empResponse == null || !empResponse.Success || empResponse.Data == null)
        {
            Console.WriteLine("❌ No se pudieron obtener empresas.");
            return;
        }

        var empresas = empResponse.Data;

        // Obtener materiales
        var matResult = await kernel.InvokeAsync("DBPlugin", "GetAllMaterials");
        var matResponse = matResult.GetValue<MaterialResponse>();

        if (matResponse == null || !matResponse.Success || matResponse.Data == null)
        {
            Console.WriteLine("❌ No se pudieron obtener materiales.");
            return;
        }

        var materiales = matResponse.Data;

        Console.WriteLine("👋 ¡Hola! Escribe 'factura' para crearla manualmente o describe tu pedido...");

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.Equals("salir", StringComparison.OrdinalIgnoreCase)) break;

            if (input.StartsWith("factura", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Introduce el ID de la empresa:");
                var idEmp = Console.ReadLine()?.Trim();
                var empresa = empresas.FirstOrDefault(e => e.Id.ToString() == idEmp);
                if (empresa == null) { Console.WriteLine("❌ Empresa no encontrada."); continue; }

                var selMat = new List<Material>();
                while (true)
                {
                    Console.WriteLine("ID del material a añadir (o 'fin'):");
                    var idMatInput = Console.ReadLine()?.Trim();
                    if (idMatInput?.Equals("fin", StringComparison.OrdinalIgnoreCase) == true) break;

                    if (int.TryParse(idMatInput, out int idMat))
                    {
                        var mat = materiales.FirstOrDefault(m => m.Id == idMat);
                        if (mat != null)
                        {
                            selMat.Add(mat);
                            Console.WriteLine($"✅ Añadido: {mat.Name}");
                        }
                        else Console.WriteLine("❌ Material no encontrado.");
                    }
                    else Console.WriteLine("❌ ID inválido.");
                }

                if (selMat.Count == 0) { Console.WriteLine("❌ No se seleccionaron materiales."); continue; }

                await MostrarFacturaAsync(kernel, empresa, selMat);
                continue;
            }

            int? tipoEmpresa = null;
            int? tipoMaterial = null;

            var matchEmpresa = Regex.Match(input, @"empresa.*tipo\s*(\d+)", RegexOptions.IgnoreCase);
            if (matchEmpresa.Success && int.TryParse(matchEmpresa.Groups[1].Value, out int tipoE))
                tipoEmpresa = tipoE;

            var matchMaterial = Regex.Match(input, @"material.*tipo\s*(\d+)", RegexOptions.IgnoreCase);
            if (matchMaterial.Success && int.TryParse(matchMaterial.Groups[1].Value, out int tipoM))
                tipoMaterial = tipoM;

            var empSel = empresas
                .Where(e => !tipoEmpresa.HasValue || e.Type == tipoEmpresa)
                .OrderBy(e => e.Price)
                .FirstOrDefault();

            var matSel = materiales
                .Where(m => (!tipoMaterial.HasValue || m.Issue == tipoMaterial) && m.Available)
                .ToList();

            if (empSel != null && matSel.Any())
            {
                await MostrarFacturaAsync(kernel, empSel, matSel);
                continue;
            }

            Console.WriteLine("❌ No se encontró una empresa o materiales válidos para generar la factura.");
        }
    }

    private static async Task MostrarFacturaAsync(Kernel kernel, Company empresa, List<Material> materiales)
    {
        var resFac = await kernel.InvokeAsync("FacturaPlugin", "GenerarFactura", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });

        Console.WriteLine("\n📄 FACTURA:");
        Console.WriteLine(resFac.GetValue<string>());

        var resIVA = await kernel.InvokeAsync("FacturaPlugin", "ObtenerIVA", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });

        Console.WriteLine("💰 DESGLOSE DE IVA:");
        Console.WriteLine(resIVA.GetValue<string>());
        Console.WriteLine("------------------------------------\n");
    }
}*/

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4.1",
            endpoint: "https://hdhdh-mdx2smel-eastus2.cognitiveservices.azure.com/",
            apiKey: "9ZMpVj9cCWRyv73s8vyxd0RL93ELHrtmNwN68ZPxRlDgBDjEgxR0JQQJ99BHACHYHv6XJ3w3AAAAACOGEv9e"
        );

        var dbPlugin = new DBPluginTest("Server=tcp:devdemoserverbbdd.database.windows.net,1433;Initial Catalog=devdemobbdd2;Persist Security Info=False;User ID=admsql;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        builder.Plugins.AddFromObject(dbPlugin, "DBPlugin");

        var facturaPlugin = new FacturaPluginTest();
        builder.Plugins.AddFromObject(facturaPlugin, "FacturaPlugin");

        var kernel = builder.Build();

        var empresasIA = await kernel.InvokeAsync("DBPlugin", "GetAllEmpresas");
        var empresasResponse = empresasIA.GetValue<EmpresaResponse>();

        if (empresasResponse == null || !empresasResponse.Success || empresasResponse.Data == null)
        {
            Console.WriteLine("❌ Error al obtener las empresas.");
            return;
        }

        var companies = empresasResponse.Data;
        var empresasJson = JsonSerializer.Serialize(companies);

        var materialesIA = await kernel.InvokeAsync("DBPlugin", "GetAllMaterials");
        var materialesResponse = materialesIA.GetValue<MaterialResponse>();

        if (materialesResponse == null || !materialesResponse.Success || materialesResponse.Data == null)
        {
            Console.WriteLine("❌ Error al obtener los materiales.");
            return;
        }

        var materials = materialesResponse.Data;
        var materialesJson = JsonSerializer.Serialize(materials);

        var promptTemplate = @"
Eres un asistente inteligente que responde preguntas sobre empresas y materiales.

Tienes la siguiente información de empresas (companies) en formato JSON:
{{$empresas}}

Cada empresa tiene propiedades como: Id, Name, Type (tipo), Price.

También tienes la siguiente información de materiales (materials) en formato JSON:
{{$materiales}}

Cada material tiene propiedades como: Id, Name, Issue (tipo), Available (disponible).

Usa esta información para responder la pregunta del usuario:
{{$pregunta}}

Responde de forma clara y útil. Si la pregunta no tiene relación con los datos, responde que no puedes ayudar.
";

        var promptFunction = kernel.CreateFunctionFromPrompt(promptTemplate);

        Console.WriteLine("👋 ¡Hola! Soy CleanFixBot. Escribe 'factura' para generar una manualmente, o hazme una pregunta.");

        while (true)
        {
            Console.Write("> ");
            string userInput = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(userInput)) continue;
            if (userInput.Equals("salir", StringComparison.OrdinalIgnoreCase)) break;

            if ((userInput.Equals("factura", StringComparison.OrdinalIgnoreCase) ||
                 userInput.Equals("crear factura", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Introduce el ID de la empresa:");
                var idEmp = Console.ReadLine()?.Trim();
                var empresa = companies.FirstOrDefault(e => e.Id.ToString() == idEmp);
                if (empresa == null) { Console.WriteLine("❌ Empresa no encontrada."); continue; }

                var selMat = new List<Material>();
                while (true)
                {
                    Console.WriteLine("ID del material a añadir (o 'fin' para terminar la factura):");
                    var idMatInput = Console.ReadLine()?.Trim();
                    if (idMatInput?.Equals("fin", StringComparison.OrdinalIgnoreCase) == true) break;

                    if (int.TryParse(idMatInput, out int idMat))
                    {
                        var mat = materials.FirstOrDefault(m => m.Id == idMat && m.Issue == empresa.Type);
                        if (mat != null)
                        {
                            selMat.Add(mat);
                            Console.WriteLine($"✅ Añadido: {mat.Name}");
                        }
                        else Console.WriteLine("❌ Material no encontrado o no pertenece a la empresa.");
                    }
                    else Console.WriteLine("❌ ID inválido.");
                }

                await MostrarFacturaAsync(kernel, empresa, selMat);
                continue;
            }
            else if (ContieneIntencionDeFactura(userInput))
            {
                var tipoEmpresa = ExtraerTipo(userInput, "empresa");
                var tipoMaterial = ExtraerTipo(userInput, "material");

                var empresa = companies
                    .Where(e => !tipoEmpresa.HasValue || e.Type == tipoEmpresa.Value)
                    .OrderBy(e => e.Price)
                    .FirstOrDefault();

                if (empresa == null)
                {
                    Console.WriteLine("❌ No se encontró una empresa válida.");
                    continue;
                }

                List<Material> materialesSeleccionados = new();

                if (!SolicitaSoloEmpresa(userInput))
                {
                    materialesSeleccionados = FiltrarMaterialesParaEmpresa(empresa, materials, tipoMaterial);

                    if (materialesSeleccionados.Count == 0)
                    {
                        Console.WriteLine("❌ No hay materiales disponibles para esta empresa.");
                        continue;
                    }

                    Console.WriteLine($"🧾 Se ha seleccionado la empresa '{empresa.Name}' y {materialesSeleccionados.Count} materiales disponibles.");
                }
                else
                {
                    Console.WriteLine($"🧾 Se ha seleccionado la empresa '{empresa.Name}' sin materiales.");
                }

                Console.WriteLine("¿Deseas generar la factura con esta información? (sí/no)");
                var confirmacion = Console.ReadLine()?.Trim().ToLower();

                if (confirmacion == "sí" || confirmacion == "si")
                {
                    await MostrarFacturaAsync(kernel, empresa, materialesSeleccionados);
                }
                else
                {
                    Console.WriteLine("❌ Factura cancelada por el usuario.");
                }

                continue;
            }

            var kernelArgs = new KernelArguments
            {
                ["empresas"] = empresasJson,
                ["materiales"] = materialesJson,
                ["pregunta"] = userInput
            };

            var responseData = await promptFunction.InvokeAsync(kernel, kernelArgs);
            Console.WriteLine($"\n🧠 CleanFixBot: {responseData.GetValue<string>()}\n");
        }
    }

    private static async Task MostrarFacturaAsync(Kernel kernel, Company empresa, List<Material> materiales)
    {
        var resFac = await kernel.InvokeAsync("FacturaPlugin", "GenerarFactura", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });

        Console.WriteLine("\n📄 FACTURA:");
        Console.WriteLine(resFac.GetValue<string>());

        var resIVA = await kernel.InvokeAsync("FacturaPlugin", "ObtenerIVA", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });

        Console.WriteLine("💰 DESGLOSE DE IVA:");
        Console.WriteLine(resIVA.GetValue<string>());
        Console.WriteLine("------------------------------------\n");
    }

    private static bool ContieneIntencionDeFactura(string input)
    {
        var palabrasClave = new[] { "hazme", "crea", "genera", "factura", "pedido", "compra" };
        return palabrasClave.Any(p => input.Contains(p, StringComparison.OrdinalIgnoreCase));
    }

    private static int? ExtraerTipo(string input, string entidad)
    {
        var sinonimos = new[] { entidad, $"{entidad}s", "productos", "insumos" };
        foreach (var palabra in sinonimos)
        {
            var match = Regex.Match(input, $@"{palabra}\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int tipo))
                return tipo;
        }
        return null;
    }

    private static List<Material> FiltrarMaterialesParaEmpresa(Company empresa, List<Material> materiales, int? tipoMaterial)
    {
        return materiales
            .Where(m =>
                m.Available &&
                (
                    tipoMaterial.HasValue
                        ? m.Issue == tipoMaterial.Value
                        : m.Issue == empresa.Type
                )
            )
            .ToList();
    }

    private static bool SolicitaSoloEmpresa(string input)
    {
        var palabrasMateriales = new[] { "material", "materiales", "producto", "productos", "insumo", "insumos" };
        return !palabrasMateriales.Any(p => input.Contains(p, StringComparison.OrdinalIgnoreCase));
    }
}
