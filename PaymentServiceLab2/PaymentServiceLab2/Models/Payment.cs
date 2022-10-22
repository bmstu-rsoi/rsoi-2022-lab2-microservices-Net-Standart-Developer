namespace PaymentServiceLab2.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public Guid Payment_UID { get; set; }
        public string Status { get; set; }
        public double Price { get; set; }
    }
}
