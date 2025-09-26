using System.ComponentModel.DataAnnotations;

namespace OrderDashboard.Database.Entities
{
    public class GlassTypes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
