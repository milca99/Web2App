using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class OrderCreateDto
    {
        [Required]
        public ItemForOrderDto Item { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int SellerId { get; set; }
    }
}
