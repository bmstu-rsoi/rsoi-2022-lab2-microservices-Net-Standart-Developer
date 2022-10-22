using GatewayLab2.Models;
using System.Text.Json;
using System.Net;

namespace GatewayLab2.Managers
{
    public class ReservationManager : Manager
    {
        public ReservationManager(string host) : base(host)
        {

        }

        public IEnumerable<Hotel> GetHotels()
        {
            string json = this.GetResources("api/v1/Reservation/GetHotels");
            IEnumerable<Hotel> hotels = JsonSerializer.Deserialize<IEnumerable<Hotel>>(json);
            return hotels;
        }

        public IEnumerable<Reservation> GetReservations()
        {
            string json = this.GetResources("api/v1/Reservation/GetReservations");
            IEnumerable<Reservation> reservations = JsonSerializer.Deserialize<IEnumerable<Reservation>>(json);
            return reservations;
        }

        public Reservation BookHotel(string username, Hotel hotel, Payment payment, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = new Reservation();
            reservation.Username = username;
            reservation.Payment_UID = payment.Payment_UID;
            reservation.Hotel_ID = hotel.ID;
            reservation.Status = "PAID";
            reservation.Start_Date = startDate;
            reservation.End_Date = endDate;

            var result = this.Post("api/v1/Reservation/CreateReservation",
                JsonSerializer.Serialize<Reservation>(reservation));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<Reservation>(result.Body);
            }

            return null;
        }

        public OperationResult CancelReservation(Guid reservationUid)
        {
            var result = this.Post($"api/v1/Reservation/CancelReservation/{reservationUid}","");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new OperationResult(true, "");
            }

            return new OperationResult(false, "Cancel error");
        }
    }
}
