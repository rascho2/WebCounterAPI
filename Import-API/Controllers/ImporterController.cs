
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace WebApplication_formstar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly string basePath;

        public FileController()
        {
            // Basisverzeichnis, kann auch aus einer Umgebungsvariablen gelesen werden
            basePath = Environment.GetEnvironmentVariable("FILE_BASE_PATH") ?? "./files";
        }

        [HttpGet("readfile")]
        public async Task<IActionResult> ReadFile([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("File name is required.");
            }

          

            if (!System.IO.File.Exists(fileName))
            {
                return NotFound("File Not Found");
            }

            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    var content = await reader.ReadToEndAsync();
                    return Ok(content);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

