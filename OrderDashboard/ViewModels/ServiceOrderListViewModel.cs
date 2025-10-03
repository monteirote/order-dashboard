namespace OrderDashboard.ViewModels
{
    public class ServiceOrderListViewModel
    {
        public int Id { get; set; }
        public string OSNumber { get; set; } = null!;
        public string? CustomerName { get; set; }
        public DateTime DueDate { get; set; }
        public int FramesCount { get; set; }
    }
}