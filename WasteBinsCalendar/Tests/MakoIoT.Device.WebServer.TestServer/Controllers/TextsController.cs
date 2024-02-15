using Microsoft.AspNetCore.Mvc;

namespace MakoIoT.Device.WebServer.TestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextsController : ControllerBase
    {

        private readonly ILogger<ConfigController> _logger;

        public TextsController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(string lang)
        {
            _logger.LogInformation($"GetText {lang}");

            var f = System.IO.File.OpenRead($"..\\..\\src\\MakoIoT.Samples.WBC.Device\\FileSystem\\texts.{lang}.json");
            return File(f, "application/json");
        }
    }
}
