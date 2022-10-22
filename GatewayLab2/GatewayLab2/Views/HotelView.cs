using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class HotelView
    {
        [JsonPropertyName("hotelUid")]
        public Guid hotelUid { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("fullAddress")]
        public string fullAddress { get; set; }

        [JsonPropertyName("stars")]
        public int stars { get; set; }


    }
}
