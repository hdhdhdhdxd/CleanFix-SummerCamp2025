using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Models;

namespace WebApi.Controllers;
[Route("api/empresa")]
[ApiController]
public class EmpresaController : ControllerBase
{
    private readonly ContextoBasedatos _context;

    public EmpresaController(ContextoBasedatos context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
    {
        var empresas = await _context.Empresas.ToListAsync();
        return Ok(empresas);
    }
}
