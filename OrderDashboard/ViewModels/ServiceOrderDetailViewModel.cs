using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrderDashboard.ViewModels
{
    public class DecorPrintDetailsViewModel
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string? Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = null!;
        public string? GlassTypeName { get; set; }
        public string? FrameTypeName { get; set; }
    }

    public class ServiceOrderDetailViewModel
    {
        public int Id { get; set; }
        public string OSNumber { get; set; } = null!;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public List<DecorPrintDetailsViewModel> DecorPrints { get; set; } = [];
    }
}