using Microsoft.AspNetCore.Mvc;

namespace MakoIoT.Device.WebServer.TestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertController : ControllerBase
    {
        private readonly ILogger<CertController> _logger;

        public CertController(ILogger<CertController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post(IFormFile httpsCertificate)
        {
            _logger.LogInformation($"Upload: {httpsCertificate.Name} {httpsCertificate.Length}");

            if (httpsCertificate == null || httpsCertificate.Length == 0)
            {
                return BadRequest("Upload a httpsCertificate.");
            }

            return Ok();
        }
    }
}
