using Microsoft.AspNetCore.Mvc;

namespace MakoIoT.Device.WebServer.TestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExitController : ControllerBase
    {

        private readonly ILogger<ConfigController> _logger;

        public ExitController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("EXIT");

            Thread.Sleep(1000);

            return Ok();
        }
    }
}
