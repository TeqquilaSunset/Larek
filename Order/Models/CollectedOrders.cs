namespace ProductOrder.Models
{
    public class CollectedOrders
    {
        public Guid Id { get; set; }
        public string NameDeliverer { get; set; }
        public Guid OrderId { get; set; }
    }
}
