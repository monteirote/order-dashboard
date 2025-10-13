using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Database.Entities;
using OrderDashboard.Database.Entities.ENUMs;
using OrderDashboard.ViewModels;

namespace OrderDashboard.Repositories
{
    public interface IServiceOrderRepository
    {
        Task<IEnumerable<ServiceOrder>> GetAllAsync();
        Task<ServiceOrder?> GetByIdWithDetailsAsync (int id);
        Task<ServiceOrder?> GetByOsNumberWithDetailsAsync (string osNumber);
        Task<ServiceOrder?> Add (ServiceOrderViewModel model);
        Task MarkAsCompleteAsync (int id);
    }

    public class ServiceOrderRepository : IServiceOrderRepository
    {
        private readonly MainContext _context;

        public ServiceOrderRepository (MainContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceOrder>> GetAllAsync ()
        {
            return await _context.ServiceOrders
                                 .Include(so => so.DecorPrints)
                                 .OrderByDescending(so => so.DueDate)
                                 .ToListAsync();
        }

        public async Task<ServiceOrder?> GetByIdWithDetailsAsync (int id)
        {
            return await _context.ServiceOrders
                                    .Include(so => so.DecorPrints).ThenInclude(dp => dp.GlassType)
                                    .Include(so => so.DecorPrints).ThenInclude(dp => dp.Frame)
                                    .FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task<ServiceOrder?> GetByOsNumberWithDetailsAsync (string osNumber)
        {
            return await _context.ServiceOrders
                                    .Include(so => so.DecorPrints).ThenInclude(dp => dp.GlassType)
                                    .Include(so => so.DecorPrints).ThenInclude(dp => dp.Frame)
                                    .FirstOrDefaultAsync(so => so.Number == osNumber);
        }

        public async Task<ServiceOrder?> Add (ServiceOrderViewModel model)
        {
            var entity = new ServiceOrder
            {
                Number = model.OSNumber,
                CreatedAt = DateTime.UtcNow,
                DueDate = model.DueDate,
                CustomerName = model.CustomerName,
                Status = Database.Entities.ENUMs.ServiceOrderStatus.InProgress
            };

            _context.ServiceOrders.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task MarkAsCompleteAsync (int id)
        {
            var serviceOrder = await _context.ServiceOrders.FindAsync(id);
            if (serviceOrder != null)
            {
                serviceOrder.Status = ServiceOrderStatus.Completed;
                await _context.SaveChangesAsync();
            }
        }
    }
}
