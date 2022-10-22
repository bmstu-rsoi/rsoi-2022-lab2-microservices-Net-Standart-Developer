using System.Text.Json;
using System.Text.Json.Serialization;
using GatewayLab2.Views;

namespace GatewayLab2.Models
{
    public class Hotel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("hotelUid")]
        public Guid HotelUID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("stars")]
        public int Stars { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        public static explicit operator HotelView(Hotel hotel)
        {
            return new HotelView()
            {
                hotelUid = hotel.HotelUID,
                name = hotel.Name,
                fullAddress = hotel.Country + ", " + hotel.City + ", " + hotel.Address,
                stars = hotel.Stars
            };
        }
    }
}
