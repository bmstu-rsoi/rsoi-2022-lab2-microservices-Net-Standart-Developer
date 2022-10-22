using System.Text.Json;
using System.Text.Json.Serialization;

namespace GatewayLab2.Models
{
    public class Reservation
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("reservation_UID")]
        public Guid Reservation_UID { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("payment_UID")]
        public Guid Payment_UID { get; set; }

        [JsonPropertyName("hotel_ID")]
        public int Hotel_ID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("start_Date")]
        public DateTime Start_Date { get; set; }

        [JsonPropertyName("end_Date")]
        public DateTime End_Date { get; set; }
    }
}
