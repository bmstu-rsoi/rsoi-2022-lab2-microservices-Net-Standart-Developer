using System.Text.Json;
using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class LoyaltyInfoResponse
    {

        [JsonPropertyName("reservationCount")]
        public int Reservation_Count { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("discount")]
        public int Discount { get; set; }
    }
}
