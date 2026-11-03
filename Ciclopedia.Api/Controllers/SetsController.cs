using Ciclopedia.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ciclopedia.Api.Controllers
{

    [ApiController]
    [Route("v1/sets")]
    public class SetsController : ControllerBase
    {
        private readonly SetsRepository _repo;
        public SetsController(SetsRepository repo) => _repo = repo;


        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            var setsLista = await _repo.ListarSets();


            return Ok(setsLista);
        }

        
    }
}
