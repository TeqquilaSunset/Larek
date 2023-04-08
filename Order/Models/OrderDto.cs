namespace ProductOrder.Models
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAdress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }

    }
}
