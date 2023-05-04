namespace ProductOrder.Models
{
    public class ShippingAdress
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public List<Order> Orders { get; set; }
    }
}
