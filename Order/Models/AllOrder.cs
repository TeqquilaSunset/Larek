using Order.Models;

namespace Order.Models
{
    public class AllOrder
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}
