using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationService2Lab.Models;

namespace ReservationService2Lab.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private ApplicationContext db;

        public ReservationController(ApplicationContext db)
        {
            this.db = db;

            if(this.db.Hotels.Count() == 0)
            {
                this.db.Hotels.Add(new Hotel()
                {
                    ID = 1,
                    Hotel_UID = new Guid("049161bb-badd-4fa8-9d90-87c9a82b0668"),
                    Name="Ararat Park Hyatt Moscow",
                    Country= "Россия",
                    City= "Москва",
                    Address= "Неглинная ул., 4",
                    Stars= 5,
                    Price= 10000
                });

                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Hotel> GetHotels()
        {
            return db.Hotels;
        }

        [HttpGet]
        public IEnumerable<Reservation> GetReservations()
        {
            return db.Reservations;
        }

        [HttpPost]
        public ActionResult<Reservation> CreateReservation(Reservation reservation)
        {
            if(reservation.ID == 0)
            {
                if(db.Reservations.Count() > 0)
                {
                    reservation.ID = db.Reservations.Max(r => r.ID) + 1;
                }
                else
                {
                    reservation.ID = 1;
                }
            }
            if(reservation.Reservation_UID == Guid.Empty)
            {
                reservation.Reservation_UID = Guid.NewGuid();
            }

            reservation.Start_Date = reservation.Start_Date.ToUniversalTime().AddHours(3);
            reservation.End_Date = reservation.End_Date.ToUniversalTime().AddHours(3);

            db.Reservations.Add(reservation);
            db.SaveChanges();

            return Ok(reservation);
        }

        [HttpPost("{reservationUid}")]
        public ActionResult CancelReservation(Guid reservationUid)
        {
            var reservation = db.Reservations.FirstOrDefault(res => res.Reservation_UID == reservationUid);
            if(reservation != null)
            {
                reservation.Status = "CANCELED";
                db.SaveChanges();
                return Ok();
            }

            return BadRequest("No such reservation");
        }
    }
}
