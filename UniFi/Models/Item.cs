using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class Item
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }


    }
}
