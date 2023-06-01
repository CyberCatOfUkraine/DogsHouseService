using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.Controllers
{
    //[Route("api/[controller]")]
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {

        [HttpGet(Name = "ping")]
        public string Get()
        {
            return "Dogs house service. Version 1.0.1";
        }
    }
}
