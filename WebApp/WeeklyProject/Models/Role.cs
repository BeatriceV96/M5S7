﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeeklyProject.Models
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
        public List<User> Users { get; set; } = [];
    }
}