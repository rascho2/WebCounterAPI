using System.Text.Json.Serialization;

namespace CounterAPI
{
    public class JsonObjectRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
