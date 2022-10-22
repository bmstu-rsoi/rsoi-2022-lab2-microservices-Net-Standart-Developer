using System.Text.Json;
using System.Text.Json.Serialization;

namespace GatewayLab2.Views
{
    public class PaginationResponse<T>
    {
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int Size { get; set; }

        public int TotalElements { get; set; }

        public T[] Items { get; set; }
    }
}
