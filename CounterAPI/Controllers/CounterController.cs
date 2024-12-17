using CounterAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WordCountAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        [HttpPost("count-words")]
        public IActionResult CountWords(JsonObjectRequest jsonrequest)
        {
            if (string.IsNullOrWhiteSpace(jsonrequest.Text))
            {
                return BadRequest("Text cannot be empty.");
            }

            // Zähle die Wörter
            var wordCount = CountWordsInText(jsonrequest);

            return Ok(new { WordCount = wordCount });
        }

        private int CountWordsInText(JsonObjectRequest text)
        {
            // Teilt den Text anhand von Leerzeichen, Zeilenumbrüchen usw. in Wörter
            var words = text.Text.Split(new[] { ' ', '\r', '\n', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }
    }
}
