namespace Backend.DTO
{
    public class OrderHistoryDto
    {
        public int Id { get; set; }
        public ItemOrderHistoryDto Item { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
    }
}
