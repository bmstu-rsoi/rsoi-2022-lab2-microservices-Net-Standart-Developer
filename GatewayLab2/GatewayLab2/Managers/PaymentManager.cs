using GatewayLab2.Models;
using System.Text.Json;
using System.Net;

namespace GatewayLab2.Managers
{
    public class PaymentManager : Manager
    {
        public PaymentManager(string host) : base(host)
        {

        }

        public IEnumerable<Payment> GetPayments()
        {
            string json = this.GetResources("api/v1/Payments/GetPayments");
            IEnumerable<Payment> payments = JsonSerializer.Deserialize<IEnumerable<Payment>>(json);
            return payments;
        }

        public double GetReservationCost(double price, DateTime start, DateTime end)
        {
            var dif = end - start;

            return price * dif.Days;
        }

        public Payment PayReservation(string status, double price)
        {
            Payment payment = new Payment();
            payment.Status = status;
            payment.Price = price;

            var result = this.Post("api/v1/Payments/CreatePayment", JsonSerializer.Serialize<Payment>(payment));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<Payment>(result.Body);
            }

            return null;
        }

        public OperationResult CancelPayment(Guid paymentUid)
        {
            var result = this.Post($"api/v1/Payments/CancelPayment/{paymentUid}", "");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new OperationResult(true, "");
            }

            return new OperationResult(false, "Cancel error");
        }
    }
}
