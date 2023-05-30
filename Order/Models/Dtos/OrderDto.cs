using ProductOrder.Enum;

namespace ProductOrder.Models.Dtos
{
    public class OrderDto
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemsDto> OrderItemsDto { get; set; }
        public Guid ShippingAddressId { get; set; }
    }
}
