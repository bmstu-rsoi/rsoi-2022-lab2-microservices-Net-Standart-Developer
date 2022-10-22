using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class UserInfoResponse
    {
        [JsonPropertyName("reservations")]
        public IEnumerable<ReservationResponse> Reservations { get; set; }

        [JsonPropertyName("loyalty")]
        public LoyaltyInfoResponse Loyalty { get; set; }
    }
}
