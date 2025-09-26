using System.ComponentModel.DataAnnotations;

namespace OrderDashboard.Database.Entities
{
    public class Frames
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
