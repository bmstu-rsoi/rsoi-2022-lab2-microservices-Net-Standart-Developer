using GatewayLab2.Models;
using System.Text.Json;
using System.Net;

namespace GatewayLab2.Managers
{
    public class LoyaltyManager : Manager
    {
        public LoyaltyManager(string host) : base(host)
        {

        }

        public IEnumerable<Loyalty> GetLoyalties()
        {
            string json = this.GetResources("api/v1/Loyalty/GetLoyalties");
            IEnumerable<Loyalty> loyalties = JsonSerializer.Deserialize<IEnumerable<Loyalty>>(json);
            return loyalties;
        }

        public int GetDiscount(Loyalty loyalty)
        {
            switch (loyalty.Status)
            {
                case "BRONZE":
                    return 5;
                case "SILVER":
                    return 7;
                case "GOLD":
                    return 10;
                default:
                    return -1;
            }
        }

        public OperationResult UpdateLoyalty(Loyalty loyalty)
        {
            var result = this.Post("api/v1/Loyalty/UpdateLoyalty", JsonSerializer.Serialize<Loyalty>(loyalty));

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new OperationResult(true, "");
            }

            return new OperationResult(false, $"Error with status code {result.StatusCode}");
        }

        public OperationResult DecrementLoyalty(string username)
        {
            var result = this.Post($"api/v1/Loyalty/DecrementLoyalty/{username}", "");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new OperationResult(true, "");
            }

            return new OperationResult(false, "Decrement loyalty error");
        }
    }
}
