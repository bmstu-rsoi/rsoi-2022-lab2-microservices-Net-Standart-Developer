using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceLab2.Models;

namespace PaymentServiceLab2.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private ApplicationContext db;

        public PaymentsController(ApplicationContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IEnumerable<Payment> GetPayments()
        {
            return db.Payments;
        }

        [HttpPost]
        public ActionResult<Payment> CreatePayment(Payment payment)
        {
            Console.WriteLine(payment.Id + " " + payment.Payment_UID + " " + payment.Price);

            if(payment == null)
            {
                return BadRequest();
            }

            if(payment.Payment_UID == Guid.Empty)
            {
                payment.Payment_UID = Guid.NewGuid();
            }

            if(payment.Id == 0)
            {
                if(db.Payments.Count() > 0)
                {
                    payment.Id = db.Payments.Max(p => p.Id) + 1;
                }
                else
                {
                    payment.Id = 1;
                }
            }

            db.Payments.Add(payment);
            db.SaveChanges();

            return Ok(payment);
        }

        [HttpPost("{paymentUid}")]
        public ActionResult CancelPayment(Guid paymentUid)
        {
            var payment = db.Payments.FirstOrDefault(payment => payment.Payment_UID == paymentUid);
            if (payment != null)
            {
                payment.Status = "CANCELED";
                db.SaveChanges();
                return Ok();
            }

            return BadRequest("No such payment");
        }
    }
}
