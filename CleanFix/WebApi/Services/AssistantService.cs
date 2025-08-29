using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using CleanFix.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

namespace WebApi.Services
{
    public class AssistantService : IAssistantService
    {
        private readonly Kernel _kernel;
        private readonly string _empresasJson;
        private readonly string _materialesJson;
        private readonly string _promptTemplate;

        public AssistantService(IConfiguration config)
        {
            // Recupera los datos de configuración
            string endpoint = config["AzureOpenAI:Endpoint"];
            string apiKey = config["AzureOpenAI:ApiKey"];
            string connectionString = config["Database:ConnectionString"];
            decimal iva = decimal.Parse(config["Bot:IVA"]);
            string moneda = config["Bot:Moneda"];
            string deploymentName = config["AzureOpenAI:Deployment"] ?? "gpt-4.1";

            //Setup del bot con Azure OpenAI

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: deploymentName,
                endpoint: endpoint,
                apiKey: apiKey
            );

            // PROMPT REVISADO: Responde de forma breve y directa
            _promptTemplate = @"Eres CleanFixBot, un asistente experto en recomendar empresas y materiales para mantenimiento y reparaciones. 

Tienes la siguiente información de empresas (companies) en JSON: {{$empresas}}
Tienes la siguiente información de materiales (materials) en JSON: {{$materiales}}

Responde de forma breve y clara. Si el usuario pide una factura, muestra solo el total, desglose de IVA y materiales. Si pregunta por empresas o materiales, responde con la información relevante en frases cortas. Si no puedes ayudar, dilo directamente. 

Pregunta: {{$pregunta}}";

            // Inicializa plugins y kernel
            var dbPlugin = new DBPluginTestPG(config.GetConnectionString("CleanFixDB"));
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();

            if (empresasResponse.Data == null || empresasResponse.Data.Count == 0)
            {
                Debug.WriteLine("[AssistantService] ¡ATENCIÓN! No se encontraron empresas en la base de datos.");
            }
            if (materialesResponse.Data == null || materialesResponse.Data.Count == 0)
            {
                Debug.WriteLine("[AssistantService] ¡ATENCIÓN! No se encontraron materiales en la base de datos.");
            }

            _empresasJson = JsonSerializer.Serialize(empresasResponse.Data ?? new List<CompanyIa>());
            _materialesJson = JsonSerializer.Serialize(materialesResponse.Data ?? new List<MaterialIa>());

            _kernel = builder.Build();
        }

        public async Task<string> ProcesarMensajeAsync(string mensaje)
        {
            // LOG para depuración
            Debug.WriteLine($"[AssistantService] Pregunta recibida: {mensaje}");
            Debug.WriteLine($"[AssistantService] Empresas JSON: {_empresasJson}");
            Debug.WriteLine($"[AssistantService] Materiales JSON: {_materialesJson}");

            // 1. Responde directamente si la petición es simple
            if (mensaje.Contains("lista empresas") || mensaje.Contains("todas las empresas") || mensaje.Trim().ToLower() == "empresas")
                return _empresasJson;
            if (mensaje.Contains("lista materiales") || mensaje.Contains("todos los materiales") || mensaje.Trim().ToLower() == "materiales")
                return _materialesJson;

            // 2. Solo llama a OpenAI si es lenguaje natural complejo
            var promptFunction = _kernel.CreateFunctionFromPrompt(_promptTemplate);
            var kernelArgs = new KernelArguments
            {
                ["empresas"] = _empresasJson,
                ["materiales"] = _materialesJson,
                ["pregunta"] = mensaje
            };

            try
            {
                var responseData = await promptFunction.InvokeAsync(_kernel, kernelArgs);
                var respuesta = responseData.GetValue<string>();
                Debug.WriteLine($"[AssistantService] Respuesta LLM: {respuesta}");
                return respuesta;
            }
            catch (Microsoft.SemanticKernel.HttpOperationException ex) when ((int)ex.StatusCode == 429)
            {
                return "Has superado el límite de peticiones. Intenta de nuevo en unos segundos.";
            }
        }
    }
}
