using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database.Entities;

namespace OrderDashboard.Database
{
    public class MainContext : DbContext
    {
        public virtual DbSet<DecorPrints> DecorPrints { get; set; }
        public virtual DbSet<GlassTypes> GlassTypes { get; set; }
        public virtual DbSet<Frames> Frames { get; set; }
        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }

        public MainContext (DbContextOptions<MainContext> options) : base (options) { }
    }
}
