using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class BookHotelView
    {
        public Guid hotelUid { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime start { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime end { get; set; }
    }
}
