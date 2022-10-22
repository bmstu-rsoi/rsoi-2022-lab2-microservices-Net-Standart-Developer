using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class CreateReservationResponse
    {
        [JsonPropertyName("reservationUid")]
        public Guid reservationUid { get; set; }

        [JsonPropertyName("hotelUid")]
        public Guid hotelUid { get; set; }

        [JsonPropertyName("startDate")]
        public string startDate { get; set; }

        [JsonPropertyName("endDate")]
        public string endDate { get; set; }

        [JsonPropertyName("discount")]
        public int discount { get; set; }

        [JsonPropertyName("status")]
        public string status { get; set; }

        public PaymentView Payment { get; set; }
    }
}
