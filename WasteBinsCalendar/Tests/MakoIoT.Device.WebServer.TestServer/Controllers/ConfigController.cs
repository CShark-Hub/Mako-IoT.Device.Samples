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

        // Example GET method to fetch configuration
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET config");

            var testData = new
            {
                wifiSettings = new { ssid = "TestSSID", password = "TestPassword" },
                calendarSettings = new { url = "https://testcalendar.com/", timeZone = "PST8PDT,M3.2.0,M11.1.0;7", httpsCertificate = (string)null },
                binNames = new
                {
                    white = "bialy",
                    brown = "brazowy",
                    yellow = "zolty",
                    green = "zielony",
                    blue = "niebieski",
                    red = "czerwony"
                }
            };

            return Ok(testData);
        }

        // Example POST method to update configuration
        [HttpPost]
        public IActionResult Post([FromBody] dynamic configData)
        {
            _logger.LogInformation("POST config");
            _logger.LogInformation($"{configData}");
            // Handle updating configuration here
            // For simplicity, this example just returns the received configData
            return Ok();
        }
    }
}
