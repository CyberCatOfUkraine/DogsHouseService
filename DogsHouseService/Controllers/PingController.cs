using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.Controllers
{
    //[Route("api/[controller]")]
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {

        [HttpGet(Name = "ping")]
        public async Task<string> Get()
        {
            return await Task.Run(() => { return "Dogs house service. Version 1.0.1"; });
        }
    }
}
