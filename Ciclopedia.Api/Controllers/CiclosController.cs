using Ciclopedia.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Ciclopedia.Api.Repositories;

[ApiController]
[Route("v1/ciclos")]
public class CiclosController : ControllerBase
{
    private readonly CiclosRepository _repo;
    public CiclosController(CiclosRepository repo) => _repo = repo;

    [HttpGet("{slug}")]
    public async Task<IActionResult> DevolveCiclo(string slug)
    {
        var CicloListado = await  _repo.BuscarCiclo(slug);

        if (CicloListado is null) return NotFound();

        return Ok(CicloListado);
     
    }




    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] CiclosQuery filtro)
    {
        var r = await _repo.ListarCiclos(filtro);
        return Ok(new { dados = r.Dados, meta = new { total = r.Total, pagina = filtro.Page, porPagina = filtro.PerPage } });
    }
}