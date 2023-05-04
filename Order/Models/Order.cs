using ProductOrder.Models;

namespace ProductOrder.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Guid ShippingAdressId { get; set; }
        public ShippingAdress ShippingAdderss { get; set; }
        public Customer Customer { get; set; }


    }
}
