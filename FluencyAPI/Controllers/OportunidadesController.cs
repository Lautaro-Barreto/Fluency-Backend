using Microsoft.AspNetCore.Mvc;

namespace FluencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OportunidadesController : ControllerBase
    {
        OportunidadesController()
        {
        }

        [HttpGet(Name = "GetOportunidadesPorEtapa")]
        public async Task<IActionResult> Get()
        {
            return await 
        }
    }
}
