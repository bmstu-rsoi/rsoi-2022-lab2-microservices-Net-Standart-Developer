using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoyaltyServiceLab2.Models;

namespace LoyaltyServiceLab2.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class LoyaltyController : ControllerBase
    {
        private ApplicationContext db;

        public LoyaltyController(ApplicationContext db)
        {
            this.db = db;

            if (this.db.Loyalties.Count() == 0)
            {
                this.db.Loyalties.Add(new Loyalty()
                {
                    ID= 1,
                    Username= "Test Max",
                    Reservation_Count= 25,
                    Status= "GOLD",
                    Discount= 10,
                });

                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Loyalty> GetLoyalties()
        {
            return db.Loyalties;
        }

        [HttpPost]
        public ActionResult UpdateLoyalty(Loyalty loyalty)
        {
            var dbLoyalty = db.Loyalties.FirstOrDefault(l => l.ID == loyalty.ID);
            if(dbLoyalty != null)
            {
                dbLoyalty.Status = loyalty.Status;
                dbLoyalty.Discount = loyalty.Discount;
                dbLoyalty.Reservation_Count = loyalty.Reservation_Count;

                db.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("{username}")]
        public ActionResult DecrementLoyalty(string username)
        {
            var loyalty = db.Loyalties.FirstOrDefault(l => l.Username == username);
            if(loyalty != null)
            {
                loyalty.Reservation_Count--;
                if(loyalty.Reservation_Count == 19)
                {
                    loyalty.Status = "SILVER";
                }
                else if(loyalty.Reservation_Count == 9)
                {
                    loyalty.Status = "BRONZE";
                }

                db.SaveChanges();
                return Ok();
            }

            return BadRequest("No such user");
        }
    }
}
