namespace ReservationService2Lab.Models
{
    public class Reservation
    {
        public int ID { get; set; }
        public Guid Reservation_UID { get; set; }
        public string Username { get; set; }
        public Guid Payment_UID { get; set; }
        public int Hotel_ID { get; set; }
        public string Status { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
}
