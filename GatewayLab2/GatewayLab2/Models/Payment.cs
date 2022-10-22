using System.Text.Json;
using System.Text.Json.Serialization;
using GatewayLab2.Views;

namespace GatewayLab2.Models
{
    public class Payment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("payment_UID")]
        public Guid Payment_UID { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        public static explicit operator PaymentView(Payment payment)
        {
            return new PaymentView()
            {
                status = payment.Status,
                price = (int)payment.Price
            };
        }
    }
}