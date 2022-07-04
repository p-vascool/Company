namespace Company.Data.Models
{
    using System.Text.Json.Serialization;
    using Company.Data.Common.Models;

    public class Destination : BaseModel<int>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("population")]
        public int Population { get; set; }
    }
}
