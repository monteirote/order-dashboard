using System.ComponentModel.DataAnnotations;

namespace OrderDashboard.Database.Entities
{
    public class Backings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
