using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;

namespace WebApi.Controllers;
[Route("api/empresa")]
[ApiController]
public class CompanyController : ControllerBase
{
    private ICompany _company;

    public CompanyController(ICompany company)
    {
        _company = company;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var list = _company.GetAll();
        // Devuelve un saludo simple
        return Ok(list);
    }
}
