using OrderDashboard.Database.Entities.ENUMs;

namespace OrderDashboard.Database.Entities
{
    public class ServiceOrder
    {   
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string? CustomerName { get; set; }
        public ServiceOrderStatus Status { get; set; } = ServiceOrderStatus.InProgress;

        public List<DecorPrints> DecorPrints { get; set; } = new List<DecorPrints>();
    }
}
