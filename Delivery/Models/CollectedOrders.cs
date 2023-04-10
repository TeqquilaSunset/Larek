namespace Delivery.Models
{
    public class CollectedOrders
    {
        public Guid Id { get; set; }
        public string NameDeliverer { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CollectedDate { get; set; }

        public bool IsCollected { get; set; } = false;
    }
}
