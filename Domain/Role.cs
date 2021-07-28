using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public User User { get; set; }
    }
}