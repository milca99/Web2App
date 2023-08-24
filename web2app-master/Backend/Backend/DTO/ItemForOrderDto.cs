using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class ItemForOrderDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ArticleId { get; set; }
    }
}
