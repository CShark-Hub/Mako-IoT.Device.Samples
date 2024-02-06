using MakoIoT.Device.LocalConfiguration.Model;
using Microsoft.AspNetCore.Mvc;

namespace MakoIoT.Device.WebServer.TestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {

        private readonly ILogger<ConfigController> _logger;

        public ConfigController(ILogger<ConfigController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET config");

            var settings = new configurationSettings()
            {
                wifiSettings = new wifiSettings { ssid = "TestSSID", password = "TestPassword" },
                calendarSettings = new calendarSettings { url = "https://testcalendar.com/", timeZone = "PST8PDT,M3.2.0,M11.1.0;7" },
                binNames = new binNames 
                {
                    white = "mixed",
                    brown = "compost",
                    yellow = "plastic",
                    green = "glass",
                    blue = "paper",
                    red = "batteries"
                }
            };
            Thread.Sleep(1000);

            return Ok(settings);
        }

        [HttpPost]
        public IActionResult Post([FromBody] configurationSettings configData)
        {
            _logger.LogInformation("POST config");
            _logger.LogInformation($"{configData}");

            Thread.Sleep(1000);

            if (configData == null)
                return BadRequest();

            return Ok();
        }
    }
}
