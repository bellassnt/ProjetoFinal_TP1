using System.Text.Json.Serialization;

namespace MusicStreamingService
{
    public class Song
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist")]
        public string Artist { get; set; }

        [JsonPropertyName("album")]
        public string Album { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }
    }
}