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

        public int? GlassTypeId { get; set; }
        public int? FrameId { get; set; }

        public string? Description { get; set; }

        public int ServiceOrderId { get; set; }


        [ForeignKey("GlassTypeId")]
        public GlassTypes? GlassType { get; set; }

        [ForeignKey("FrameId")]
        public Frames? Frame { get; set; }

        public virtual ServiceOrder ServiceOrder { get; set; }
    }
}
