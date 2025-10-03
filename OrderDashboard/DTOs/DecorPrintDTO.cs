namespace OrderDashboard.DTOs
{
    public record DecorPrintDTO
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string? ImageUrl { get; set; }
        public int? GlassTypeId { get; set; }
        public int? FrameId { get; set; }
        public string? Description { get; set; }
        public int ServiceOrderId { get; set; }
    }
}
