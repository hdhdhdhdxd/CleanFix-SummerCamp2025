using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using CleanFix.Plugins;
using System.Collections.Generic;

namespace WebApi.Services
{
    public interface IAssistantService
    {
        Task<string> ProcesarMensajeAsync(string mensaje);
    }

    public class AssistantService : IAssistantService
    {
        private readonly Kernel _kernel;
        private readonly string _empresasJson;
        private readonly string _materialesJson;
        private readonly string _promptTemplate = @"
Eres un asistente inteligente que responde preguntas sobre empresas y materiales.

Tienes la siguiente información de empresas (companies) en formato JSON: {{$empresas}} Cada empresa tiene propiedades como: Id, Name, Type (tipo), Price.

También tienes la siguiente información de materiales (materials) en formato JSON: {{$materiales}} Cada material tiene propiedades como: Id, Name, Issue (tipo), Available (disponible).

Usa esta información para responder la pregunta del usuario: {{$pregunta}} Responde de forma clara y útil.

Si la pregunta no tiene relación con los datos, responde que no puedes ayudar.
";

        public AssistantService(IConfiguration config)
        {
            var endpoint = config["AzureOpenAI:Endpoint"];
            var apiKey = config["AzureOpenAI:ApiKey"];
            var connectionString = config.GetConnectionString("DefaultConnection");

            var builder = Kernel.CreateBuilder();
            builder.AddAzureOpenAIChatCompletion(
                deploymentName: "gpt-4.1",
                endpoint: endpoint,
                apiKey: apiKey
            );

            // Carga datos de empresas y materiales al inicializar
            var dbPlugin = new DBPluginTestPG(connectionString);
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();

            _empresasJson = JsonSerializer.Serialize(empresasResponse.Data ?? new List<CompanyIa>());
            _materialesJson = JsonSerializer.Serialize(materialesResponse.Data ?? new List<MaterialIa>());

            _kernel = builder.Build();
        }

        public async Task<string> ProcesarMensajeAsync(string mensaje)
        {
            var promptFunction = _kernel.CreateFunctionFromPrompt(_promptTemplate);

            var kernelArgs = new KernelArguments
            {
                ["empresas"] = _empresasJson,
                ["materiales"] = _materialesJson,
                ["pregunta"] = mensaje
            };

            var responseData = await promptFunction.InvokeAsync(_kernel, kernelArgs);
            return responseData.GetValue<string>();
        }
    }
}
