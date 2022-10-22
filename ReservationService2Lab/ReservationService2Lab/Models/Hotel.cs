using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReservationService2Lab.Models
{
    public class Hotel
    {
        public int ID { get; set; }

        [JsonPropertyName("hotelUid")]
        public Guid Hotel_UID { get; set; }

        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Stars { get; set; }
        public double Price { get; set; }
    }
}
