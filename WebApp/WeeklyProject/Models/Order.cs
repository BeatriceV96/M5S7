using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyProject.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required, StringLength(255)]
        public string ShippingAddress { get; set; }

        public string Notes { get; set; }

        public bool IsProcessed { get; set; } = false;

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
