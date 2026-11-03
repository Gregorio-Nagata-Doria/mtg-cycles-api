using Ciclopedia.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ciclopedia.Api.Controllers
{
 


        [ApiController]
        [Route("v1/anos")]
        public class AnosController : ControllerBase
    {


        private readonly AnosRepository _repo;

        public AnosController(AnosRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> ListarAnos()
        {


            var anosLista = await _repo.ListarAnos();


            return Ok(anosLista);
        }
    }

    
}
