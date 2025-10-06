namespace OrderDashboard.ViewModels
{
    public class DashboardQuadroViewModel
    {
        public string ImageUrl { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string? FrameTypeName { get; set; }
        public string? GlassTypeName { get; set; }
        public string Description { get; set; }
    }

    public class DashboardOSViewModel
    {
        public string OSNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime DueDate { get; set; }
        public List<DashboardQuadroViewModel> Quadros { get; set; } = [];
    }
}
