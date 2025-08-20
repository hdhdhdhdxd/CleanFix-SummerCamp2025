using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using System.Globalization;


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



class Program
{
    static async Task Main(string[] args)
    {
        //Cifrado de configuración para Azure OpenAI y la base de datos
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettingsIaBot.json")
            .Build();

        string endpoint = config["AzureOpenAI:Endpoint"];
        string apiKey = config["AzureOpenAI:ApiKey"];
        string connectionString = config["Database:ConnectionString"];
        decimal iva = decimal.Parse(config["Bot:IVA"]);
        string moneda = config["Bot:Moneda"];


        // Configura la codificación de salida de la consola para UTF-8
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Inicializa el constructor del kernel de Semantic Kernel
        var builder = Kernel.CreateBuilder();

        // Configura el servicio de Azure OpenAI para generación de texto
        builder.AddAzureOpenAIChatCompletion(
            deploymentName: "gpt-4.1",
            endpoint: endpoint,
            apiKey: apiKey
        );

        // Agrega el plugin de base de datos para consultar empresas y materiales
        var dbPlugin = new DBPluginTest(connectionString);

        builder.Plugins.AddFromObject(dbPlugin, "DBPlugin");

        // Agrega el plugin de facturación para generar facturas
        var facturaPlugin = new FacturaPluginTest();
        builder.Plugins.AddFromObject(facturaPlugin, "FacturaPlugin");

        // Construye el kernel con los plugins y servicios configurados
        var kernel = builder.Build();

        // Invoca la función del plugin para obtener todas las empresas
        var empresasIA = await kernel.InvokeAsync("DBPlugin", "GetAllEmpresas");
        var empresasResponse = empresasIA.GetValue<EmpresaResponse>();

        // Verifica si la respuesta es válida
        if (empresasResponse == null || !empresasResponse.Success || empresasResponse.Data == null)
        {
            Console.WriteLine(" ❌ Error al obtener las empresas.");
            return;
        }

        // Serializa la lista de empresas a formato JSON
        var companies = empresasResponse.Data;
        var empresasJson = JsonSerializer.Serialize(companies);

        // Invoca la función del plugin para obtener todos los materiales
        var materialesIA = await kernel.InvokeAsync("DBPlugin", "GetAllMaterials");
        var materialesResponse = materialesIA.GetValue<MaterialResponse>();

        // Verifica si la respuesta es válida
        if (materialesResponse == null || !materialesResponse.Success || materialesResponse.Data == null)
        {
            Console.WriteLine(" ❌ Error al obtener los materiales.");
            return;
        }

        // Serializa la lista de materiales a formato JSON
        var materials = materialesResponse.Data;
        var materialesJson = JsonSerializer.Serialize(materials);

        // Define el prompt que usará el modelo para responder preguntas
        var promptTemplate = @" Eres un asistente inteligente que responde preguntas sobre empresas y materiales. 

        Tienes la siguiente información de empresas (companies) en formato JSON: {{$empresas}} Cada empresa tiene propiedades como: Id, Name, Type (tipo), Price. 

        También tienes la siguiente información de materiales (materials) en formato JSON: {{$materiales}} Cada material tiene propiedades como: Id, Name, Issue (tipo), Available (disponible). 

        Usa esta información para responder la pregunta del usuario: {{$pregunta}} Responde de forma clara y útil. 

        Si la pregunta no tiene relación con los datos, responde que no puedes ayudar. ";

        // Crea una función del kernel basada en el prompt definido
        var promptFunction = kernel.CreateFunctionFromPrompt(promptTemplate);

        // Mensaje de bienvenida del chatbot
        Console.WriteLine(" 👋 ¡Hola! Soy CleanFixBot. Escribe 'factura' para generar una manualmente, o hazme una pregunta.");

        // Bucle principal del chatbot
        while (true)
        {
            Console.Write("> ");
            string userInput = Console.ReadLine()?.Trim();

            // Ignora entradas vacías
            if (string.IsNullOrEmpty(userInput)) continue;

            // Normaliza el texto del usuario para facilitar el análisis
            userInput = userInput.Normalize(NormalizationForm.FormC);
            userInput = Regex.Replace(userInput, @"\s+", " ");
            userInput = userInput.ToLowerInvariant();

            // Salir del chatbot si el usuario lo indica
            if (userInput.Equals("salir", StringComparison.OrdinalIgnoreCase)) break;

            // Modo manual de creación de factura
            if (userInput.Equals("factura") || userInput.Equals("crear factura"))
            {
                Console.WriteLine(" Introduce el ID de la empresa:");
                var idEmp = Console.ReadLine()?.Trim();
                var empresa = companies.FirstOrDefault(e => e.Id.ToString() == idEmp);
                if (empresa == null) { Console.WriteLine(" ❌ Empresa no encontrada."); continue; }

                var selMat = new List<Material>();
                while (true)
                {
                    Console.WriteLine(" ID del material a añadir (o 'fin' para terminar la factura):");
                    var idMatInput = Console.ReadLine()?.Trim();
                    if (idMatInput?.Equals("fin", StringComparison.OrdinalIgnoreCase) == true) break;

                    // Añade materiales seleccionados por el usuario
                    if (int.TryParse(idMatInput, out int idMat))
                    {
                        var mat = materials.FirstOrDefault(m => m.Id == idMat);
                        if (mat != null)
                        {
                            selMat.Add(mat);
                            Console.WriteLine($" ✅ Añadido: {mat.Name}");
                        }
                        else Console.WriteLine(" ❌ Material no encontrado.");
                    }
                    else Console.WriteLine(" ❌ ID inválido.");
                }

                // Genera la factura con los datos seleccionados
                await MostrarFacturaAsync(kernel, empresa, selMat);
                continue;
            }
            // Modo automático de creación de factura basado en intención del usuario
            else if (ContieneIntencionDeFactura(userInput))
            {
                var tipoEmpresa = ExtraerTipo(userInput, "empresa");
                var tipoMaterial = ExtraerTipo(userInput, "material");

                // Selecciona la empresa más barata del tipo indicado
                var empresa = companies
                    .Where(e => !tipoEmpresa.HasValue || e.Type == tipoEmpresa.Value)
                    .OrderBy(e => e.Price)
                    .FirstOrDefault();

                if (empresa == null)
                {
                    Console.WriteLine(" ❌ No se encontró una empresa válida.");
                    continue;
                }

                List<Material> materialesSeleccionados = new();

                // Determina qué materiales incluir según la intención del usuario
                if (SolicitaSinMateriales(userInput))
                {
                    Console.WriteLine($" 🧾 Se ha seleccionado la empresa '{empresa.Name}' sin materiales.");
                }
                else if (SolicitaTodosMateriales(userInput))
                {
                    materialesSeleccionados = materials.Where(m => m.Available).ToList();
                    Console.WriteLine($" 🧾 Se han seleccionado todos los materiales disponibles ({materialesSeleccionados.Count}).");
                }
                else
                {
                    if (!tipoMaterial.HasValue)
                    {
                        Console.WriteLine(" ❌ No se pudo determinar el tipo de material.");
                        continue;
                    }

                    if (SolicitaMaterialMasCaro(userInput))
                    {
                        materialesSeleccionados = SeleccionarMaterialMasCaro(materials, tipoMaterial.Value);

                        if (materialesSeleccionados.Count == 0)
                        {
                            Console.WriteLine(" ❌ No hay materiales disponibles del tipo solicitado.");
                            continue;
                        }

                        Console.WriteLine($" 🧾 Se han seleccionado la empresa y el material más caro del tipo {tipoMaterial.Value}.");
                    }
                    else if (SolicitaMaterialMasBarato(userInput))
                    {
                        materialesSeleccionados = SeleccionarMaterialMasBarato(materials, tipoMaterial.Value);

                        if (materialesSeleccionados.Count == 0)
                        {
                            Console.WriteLine(" ❌ No hay materiales disponibles del tipo solicitado.");
                            continue;
                        }

                        Console.WriteLine($" 🧾 Se han seleccionado la empresa y el material más barato del tipo {tipoMaterial.Value}.");
                    }
                    else
                    {
                        materialesSeleccionados = FiltrarMateriales(materials, tipoMaterial.Value);

                        if (materialesSeleccionados.Count == 0)
                        {
                            Console.WriteLine(" ❌ No hay materiales disponibles del tipo solicitado.");
                            continue;
                        }

                        Console.WriteLine($" 🧾 Se han seleccionado {materialesSeleccionados.Count} materiales del tipo {tipoMaterial.Value}.");
                    }
                }

                // Muestra resumen de selección y solicita confirmación
                Console.WriteLine($" 🔍 Empresa: {empresa.Name}");
                Console.WriteLine($" 🔍 Materiales: {string.Join(", ", materialesSeleccionados.Select(m => m.Name))}");

                Console.WriteLine(" ¿Deseas generar la factura con esta información? (sí/no)");
                var confirmacion = Console.ReadLine()?.Trim().ToLower();

                if (confirmacion == "sí" || confirmacion == "si")
                {
                    await MostrarFacturaAsync(kernel, empresa, materialesSeleccionados);
                }
                else
                {
                    Console.WriteLine(" ❌ Factura cancelada por el usuario.");
                }

                continue;
            }

            // Si no es una factura, se trata como una pregunta general
            var kernelArgs = new KernelArguments
            {
                ["empresas"] = empresasJson,
                ["materiales"] = materialesJson,
                ["pregunta"] = userInput
            };

            // Invoca el modelo con los datos y la pregunta del usuario
            var responseData = await promptFunction.InvokeAsync(kernel, kernelArgs);
            Console.WriteLine($"\n🧠 CleanFixBot: {responseData.GetValue<string>()}\n");
        }
    }


    // Genera y muestra una factura junto con el desglose de IVA usando el plugin de facturación
    private static async Task MostrarFacturaAsync(Kernel kernel, Company empresa, List<Material> materiales)
    {
        // Invoca la función del plugin para generar la factura
        var resFac = await kernel.InvokeAsync("FacturaPlugin", "GenerarFactura", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });

        Console.WriteLine("\n 📄 FACTURA:");
        Console.WriteLine(resFac.GetValue<string>());

        // Invoca la función del plugin para obtener el desglose de IVA
        var resIVA = await kernel.InvokeAsync("FacturaPlugin", "ObtenerIVA", new()
        {
            ["empresa"] = empresa,
            ["materiales"] = materiales
        });
        Console.WriteLine(" 💰 DESGLOSE DE IVA:");
        Console.WriteLine(resIVA.GetValue<string>());
        Console.WriteLine("");
        Console.WriteLine(" ------------------------------------\n");
    }

    // Detecta si el texto del usuario contiene intención de generar una factura
    private static bool ContieneIntencionDeFactura(string input)
    {
        var palabrasClave = new[] { "hazme", "crea", "genera", "factura", "pedido", "compra" };
        return palabrasClave.Any(p => input.Contains(p, StringComparison.OrdinalIgnoreCase));
    }

    // Extrae el tipo (número) de empresa o material desde el texto del usuario
    private static int? ExtraerTipo(string input, string entidad)
    {
        // Busca coincidencias como "empresa tipo 2" o "material del tipo 3"
        var match = Regex.Match(input, $@"{entidad}\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*(\d+)", RegexOptions.IgnoreCase);
        if (match.Success && int.TryParse(match.Groups[1].Value, out int tipo))
            return tipo;

        // Validación adicional específica para materiales
        if (entidad == "material")
        {
            var matchMaterial = Regex.Match(input, @"material\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (matchMaterial.Success && int.TryParse(matchMaterial.Groups[1].Value, out tipo))
                return tipo;
        }

        return null;
    }

    // Filtra los materiales disponibles según el tipo indicado
    private static List<Material> FiltrarMateriales(List<Material> materiales, int? tipoMaterial)
    {
        return materiales
            .Where(m => m.Available && (!tipoMaterial.HasValue || m.Issue == tipoMaterial.Value))
            .ToList();
    }

    // Detecta si el usuario quiere generar una factura sin incluir materiales
    private static bool SolicitaSinMateriales(string input)
    {
        var frases = new[]
        {
        "sin materiales",
        "solo empresa",
        "no quiero materiales",
        "empresa sin productos"
    };

        return frases.Any(f => input.Contains(f, StringComparison.OrdinalIgnoreCase));
    }

    // Detecta si el usuario quiere incluir todos los materiales disponibles
    private static bool SolicitaTodosMateriales(string input)
    {
        var frases = new[]
        {
        "todos los materiales",
        "materiales disponibles",
        "todo lo que haya",
        "todos los productos",
        "todo lo disponible"
    };

        return frases.Any(f => input.Contains(f, StringComparison.OrdinalIgnoreCase));
    }

    // Detecta si el usuario solicita el material más barato de un tipo específico
    private static bool SolicitaMaterialMasBarato(string input)
    {
        // Normaliza el texto para facilitar el análisis
        input = input.ToLowerInvariant();
        input = input.Normalize(NormalizationForm.FormC);
        input = Regex.Replace(input, @"\s+", " ");

        // Patrones que indican intención de buscar el más barato
        var patrones = new[]
        {
        @"material\s+(de\s+tipo|del\s+tipo|tipo)?\s*\d+\s+(más barato|mas barato)",
        @"el\s+material\s+(más barato|mas barato)",
        @"producto\s+(más barato|mas barato)",
        @"insumo\s+(más barato|mas barato)",
        @"el\s+(más barato|mas barato)"
    };

        return patrones.Any(p => Regex.IsMatch(input, p));
    }

    // Detecta si el usuario solicita el material más caro de un tipo específico
    private static bool SolicitaMaterialMasCaro(string input)
    {
        // Normaliza el texto para facilitar el análisis
        input = input.ToLowerInvariant();
        input = input.Normalize(NormalizationForm.FormC);
        input = Regex.Replace(input, @"\s+", " ");

        // Patrones que indican intención de buscar el más caro/cara
        var patrones = new[]
        {
            @"material\s+(de\s+tipo|del\s+tipo|tipo)?\s*\d+\s+(más caro|mas caro|más cara|mas cara)",
            @"el\s+material\s+(más caro|mas caro|más cara|mas cara)",
            @"producto\s+(más caro|mas caro|más cara|mas cara)",
            @"insumo\s+(más caro|mas caro|más cara|mas cara)",
            @"el\s+(más caro|mas caro|más cara|mas cara)"
        };

        return patrones.Any(p => Regex.IsMatch(input, p));
    }

    // Selecciona el material más barato disponible del tipo indicado
    private static List<Material> SeleccionarMaterialMasBarato(List<Material> materiales, int tipo)
    {
        var materialMasBarato = materiales
            .Where(m => m.Available && m.Issue == tipo)
            .OrderBy(m => m.Cost)
            .FirstOrDefault();

        // Devuelve el material más barato como lista (o vacía si no hay)
        return materialMasBarato != null ? new List<Material> { materialMasBarato } : new List<Material>();
    }

    // Selecciona el material más caro disponible del tipo indicado
    private static List<Material> SeleccionarMaterialMasCaro(List<Material> materiales, int tipo)
    {
        var materialMasCaro = materiales
            .Where(m => m.Available && m.Issue == tipo)
            .OrderByDescending(m => m.Cost)
            .FirstOrDefault();

        // Devuelve el material más caro como lista (o vacía si no hay)
        return materialMasCaro != null ? new List<Material> { materialMasCaro } : new List<Material>();
    }
}

