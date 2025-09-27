using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderDashboard.Database.Entities
{
    public class DecorPrints
    {
        [Key]
        public int Id { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }
        public string? ImageUrl { get; set; }

        public int? PaperId { get; set; }
        public int? GlassTypeId { get; set; }
        public int? FrameId { get; set; }
        public int? BackingId { get; set; }


        [ForeignKey("PaperId")]
        public Papers? Paper { get; set; }

        [ForeignKey("GlassTypeId")]
        public GlassTypes? GlassType { get; set; }

        [ForeignKey("FrameId")]
        public Frames? Frame { get; set; }

        [ForeignKey("BackingId")]
        public Backings? Backing { get; set; }
    }
}
