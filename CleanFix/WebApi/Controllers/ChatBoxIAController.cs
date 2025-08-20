using CleanFixConsola.PluginsIATest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
namespace WebApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ChatBoxIAController : ControllerBase
{
    private readonly BotService _bot;

    public ChatBoxIAController()
    {
        _bot = new BotService();
    }

    [HttpPost]
    public IActionResult Post([FromBody] ChatRequest request)
    {
        var respuesta = _bot.ProcesarMensaje(request.Mensaje);
        return Ok(new ChatResponse { Respuesta = respuesta });
    }
}

