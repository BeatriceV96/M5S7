using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyProject.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(20)]
        public required string Password { get; set; }
        public List<Role> Roles { get; set; } = [];

    }
}
