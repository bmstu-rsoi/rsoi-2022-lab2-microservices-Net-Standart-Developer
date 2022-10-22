using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GatewayLab2.Models;
using GatewayLab2.Managers;
using GatewayLab2.Views;

namespace GatewayLab2.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        string reservationService { get; }
        ReservationManager ReservationManager { get; }

        string paymentService { get; }
        PaymentManager PaymentManager { get; }

        string loyaltyService { get; }
        LoyaltyManager LoyaltyManager { get; }

        public GatewayController()
        {
            reservationService = Environment.GetEnvironmentVariable("RESERVATION");
            ReservationManager = new ReservationManager(reservationService);

            paymentService = Environment.GetEnvironmentVariable("PAYMENT");
            PaymentManager = new PaymentManager(paymentService);

            loyaltyService = Environment.GetEnvironmentVariable("LOYALTY");
            LoyaltyManager = new LoyaltyManager(loyaltyService);
        }

        [HttpGet("hotels")]
        public ActionResult<IEnumerable<Hotel>> Hotels(int page, int size)
        {
            var hotels = this.ReservationManager.GetHotels();
            PaginationResponse<Hotel> hotelsResponse = new PaginationResponse<Hotel>();
            hotelsResponse.Page = page;
            hotelsResponse.Size = size;
            hotelsResponse.Items = hotels.ToArray();
            hotelsResponse.TotalElements = hotels.Count();
            return Ok(hotelsResponse);
        }

        [HttpGet("me")]
        public ActionResult<UserInfoResponse> GetUserInfo()
        {
            string username = this.Request.Headers["X-User-Name"];
            var loyalty = LoyaltyManager.GetLoyalties()
                                        .FirstOrDefault(loyalty => loyalty.Username == username);
            var reservations = ReservationManager.GetReservations()
                                                 .Where(reser => reser.Username == username);

            var hotels = ReservationManager.GetHotels();
            var payments = PaymentManager.GetPayments();

            List<ReservationResponse> responseList = new List<ReservationResponse>();
            foreach (var reservation in reservations)
            {
                var hotel = hotels.FirstOrDefault(h => h.ID == reservation.Hotel_ID);
                var payment = payments.FirstOrDefault(p => p.Payment_UID == reservation.Payment_UID);

                ReservationResponse response = new ReservationResponse();
                response.reservationUid = reservation.Reservation_UID;
                response.Hotel = (HotelView)hotel;
                response.startDate = reservation.Start_Date.ToString("yyyy-MM-dd");
                response.endDate = reservation.End_Date.ToString("yyyy-MM-dd");
                response.status = reservation.Status;
                response.Payment = (PaymentView)payment;

                responseList.Add(response);
            }

            if (loyalty != null)
            {
                return Ok(new UserInfoResponse() { Loyalty = (LoyaltyInfoResponse)loyalty, Reservations = responseList});
            }

            return BadRequest();
        }

        [HttpGet("reservations")]
        public ActionResult<IEnumerable<ReservationResponse>> GetReservations()
        {
            string username = this.Request.Headers["X-User-Name"];
            var reservations = ReservationManager.GetReservations()
                                                 .Where(reser => reser.Username == username);

            var hotels = ReservationManager.GetHotels();
            var payments = PaymentManager.GetPayments();

            List<ReservationResponse> responseList = new List<ReservationResponse>();
            foreach (var reservation in reservations)
            {
                var hotel = hotels.FirstOrDefault(h => h.ID == reservation.Hotel_ID);
                var payment = payments.FirstOrDefault(p => p.Payment_UID == reservation.Payment_UID);

                ReservationResponse response = new ReservationResponse();
                response.reservationUid = reservation.Reservation_UID;
                response.Hotel = (HotelView)hotel;
                response.startDate = reservation.Start_Date.ToString("yyyy-MM-dd");
                response.endDate = reservation.End_Date.ToString("yyyy-MM-dd");
                response.status = reservation.Status;
                response.Payment = (PaymentView)payment;

                responseList.Add(response);
            }

            if (responseList.Count > 0)
            {
                return Ok(responseList);
            }
            
            return Ok();
        }

        [HttpGet("reservations/{reservationUid}")]
        public ActionResult<ReservationResponse> GetReservation(Guid reservationUid)
        {
            string username = this.Request.Headers["X-User-Name"];
            var reservation = ReservationManager.GetReservations()
                                                .FirstOrDefault(res => res.Reservation_UID == reservationUid &&
                                                                       res.Username == username);
            var hotel = ReservationManager.GetHotels()
                                          .FirstOrDefault(hotel => hotel.ID == reservation?.Hotel_ID);
            var payment = PaymentManager.GetPayments()
                                          .FirstOrDefault(payment => payment.Payment_UID == reservation?.Payment_UID);
            if (reservation != null && hotel != null && payment != null)
            {
                ReservationResponse response = new ReservationResponse();
                response.reservationUid = reservation.Reservation_UID;
                response.Hotel = (HotelView)hotel;
                response.startDate = reservation.Start_Date.ToString("yyyy-MM-dd");
                response.endDate = reservation.End_Date.ToString("yyyy-MM-dd");
                response.status = reservation.Status;
                response.Payment = (PaymentView)payment;

                return Ok(response);
            }

            return NotFound();
        }

        [HttpPost("reservations")]
        public ActionResult BookHotel(BookHotelView view)
        {
            string username = this.Request.Headers["X-User-Name"];
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            var hotel = ReservationManager.GetHotels().FirstOrDefault(hotel => hotel.HotelUID == view.hotelUid);
            if (hotel == null)
            {
                return BadRequest("No such hotel");
            }

            var loyalty = LoyaltyManager.GetLoyalties()
                                        .FirstOrDefault(loyalty => loyalty.Username == username);
            if(loyalty == null)
            {
                return BadRequest();
            }


            double clientCost = PaymentManager.GetReservationCost(hotel.Price, view.start, view.end);
            int discount = LoyaltyManager.GetDiscount(loyalty);
            clientCost = clientCost - (clientCost / 100 * discount);

            var payment = PaymentManager.PayReservation("PAID", clientCost);

            loyalty.Reservation_Count++;
            if(loyalty.Reservation_Count >= 10)
            {
                loyalty.Status = "SILVER";
            }
            if (loyalty.Reservation_Count >= 20)
            {
                loyalty.Status = "GOLD";
            }
            LoyaltyManager.UpdateLoyalty(loyalty);

            var reservation = ReservationManager.BookHotel(username, hotel, payment, view.start, view.end);

            CreateReservationResponse response = new CreateReservationResponse();
            response.reservationUid = reservation.Reservation_UID;
            response.hotelUid = hotel.HotelUID;
            response.startDate = view.start.ToString("yyyy-MM-dd");
            response.endDate = view.end.ToString("yyyy-MM-dd");
            response.discount = loyalty.Discount;
            response.status = reservation.Status;
            response.Payment = new PaymentView() { price = (int)payment.Price, status = payment.Status };
            return Ok(response);
        }

        [HttpDelete("reservations/{reservationUid}")]
        public ActionResult CancelReservation(Guid reservationUid)
        {
            var reservation = ReservationManager.GetReservations()
                                                .FirstOrDefault(res => res.Reservation_UID == reservationUid);

            if(reservation == null)
            {
                return NotFound("no such reservation");
            }

            if (ReservationManager.CancelReservation(reservationUid).Success &&
                PaymentManager.CancelPayment(reservation.Payment_UID).Success &&
                LoyaltyManager.DecrementLoyalty(reservation.Username).Success)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpGet("loyalty")]
        public ActionResult<Loyalty> GetLoyalty()
        {
            string username = this.Request.Headers["X-User-Name"];
            var loyalty = LoyaltyManager.GetLoyalties()
                                        .FirstOrDefault(loyalty => loyalty.Username == username);

            if (loyalty != null)
            {
                return Ok((LoyaltyInfoResponse)loyalty);
            }

            return BadRequest();
        }
    }
}