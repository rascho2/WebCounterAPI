using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Text;
using System;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public SchedulerController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet("run-scheduler")]
    public async Task<IActionResult> RunScheduler()
    {
        try
        {
            // Step 1: Redirect to Import API
            var importResponse = await _httpClient.GetAsync("http://localhost:5001/api/File/readfile?fileName=TextFileUebung.txt");
            if (!importResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)importResponse.StatusCode, "Failed to call Import API");
            }
            string importcontent = await importResponse.Content.ReadAsStringAsync();
            var jsonObjectRequest = new { text = importcontent };
            string jsonData = JsonSerializer.Serialize(jsonObjectRequest);
            string url_countapi = "http://localhost:5002/api/Import/count-words";

            _httpClient.DefaultRequestHeaders.Add("accept", "*/*");

            //string import_json = "\""+erg+"\"";
            
            var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            // Step 2: Redirect to Counter API
            HttpResponseMessage counterResponse = await _httpClient.PostAsync(url_countapi, jsonContent);

            if (!counterResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)counterResponse.StatusCode, "Failed to call Counter API");
            }

            string counter_body = await counterResponse.Content.
                ReadAsStringAsync();
            var exportjson = new { text = counter_body };
            string exportdata = JsonSerializer.Serialize(exportjson);

            string url_export = "http://localhost:5003/api/File/write-text";

            _httpClient.DefaultRequestHeaders.Add("accept", "*/*");

            var jsonexport = new StringContent(exportdata, Encoding.UTF8, "application/json");
            // Step 3: Redirect to Export API
            HttpResponseMessage exportResponse = await _httpClient.PostAsync(url_export,jsonexport);

            if (!exportResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)exportResponse.StatusCode, "Failed to call Export API");
            }

            return Ok("All APIs executed successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("redirect-to-import")]
    public IActionResult RedirectToImport()
    {
        // Step 1: Redirect client to Import API
        return Redirect("http://localhost:5001/api/File/readfile?fileName=TextFileUebung.txt");
    }

    [HttpGet("redirect-to-counter")]
    public IActionResult RedirectToCounter()
    {
        // Step 2: Redirect client to Counter API
        return Redirect("http://localhost:5002/swagger");
    }

    [HttpGet("redirect-to-export")]
    public IActionResult RedirectToExport()
    {
        // Step 3: Redirect client to Export API
        return Redirect("http://localhost:5003/swagger");
    }
}
