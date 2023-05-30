using ProductOrder.Enum;
using ProductOrder.Models;

namespace ProductOrder.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderEnum OrderStatus { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }


    }
}
