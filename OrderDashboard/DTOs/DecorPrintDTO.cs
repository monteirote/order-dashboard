namespace OrderDashboard.DTOs
{
    public class DecorPrintDTO
    {
        public int Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string? ImageUrl { get; set; }
        public string? Paper { get; set; }
        public string? GlassType { get; set; }
        public string? Frame { get; set; }
        public string? Backing { get; set; }
    }
}
