using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Numerics;
using System.Security;
using Export_API;

namespace Export_API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    [HttpPost("write-text")]
    public IActionResult WriteText(ExportJsonClass jsonRequest)
    {
        if (string.IsNullOrWhiteSpace(jsonRequest.Text))
        {
            return BadRequest("Text cannot be null or empty.");
        }
        //return Ok(jsonRequest.Text);
        try
        {
            // Use a directory inside the container
            string containerPath = @"\app\MyWebApiTextFile.txt";

            // Define the file name
            string fileName = "MyWebApiTextFile.txt";

            // Combine the container directory and file name
            string filePath = Path.Combine(containerPath, fileName);
            //return Ok(filePath);
            //Console.WriteLine(jsonRequest.Text);
            // Write the text to the file
            string filetext = jsonRequest.Text;
            System.IO.File.WriteAllText(containerPath, filetext);
            

            // Return the file path in the response
            return Ok($"File successfully written to: {filePath}");
        }
        catch (Exception ex)
        {
            // Log the error if needed (optional)
            Console.WriteLine(ex.Message);

            return StatusCode(500, "An error occurred while writing the file.");
        }
    }
}
