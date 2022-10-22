using System.Text.Json;
using System.Text.Json.Serialization;
using GatewayLab2.Views;

namespace GatewayLab2.Models
{
    public class Loyalty
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("reservation_Count")]
        public int Reservation_Count { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("discount")]
        public int Discount { get; set; }

        public static explicit operator LoyaltyInfoResponse(Loyalty loyalty)
        {
            return new LoyaltyInfoResponse() { Discount = loyalty.Discount,
                Status = loyalty.Status,
                Reservation_Count = loyalty.Reservation_Count };
        }
    }
}
