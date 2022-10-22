namespace LoyaltyServiceLab2.Models
{
    public class Loyalty
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public int Reservation_Count { get; set; }
        public string Status { get; set; }
        public int Discount { get; set; }
    }
}
