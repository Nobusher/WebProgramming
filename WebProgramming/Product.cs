using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebProgramming
{
    public class Product
    {
        [JsonPropertyName("product_id")]
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public decimal CostPrice { get; set;}
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Comment {  get; set;}
    }
}
