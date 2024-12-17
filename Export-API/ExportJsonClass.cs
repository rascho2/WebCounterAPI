using System.Text.Json.Serialization;

namespace Export_API
{
    public class ExportJsonClass
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
