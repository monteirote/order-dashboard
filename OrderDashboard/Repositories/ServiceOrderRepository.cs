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
        Task DeleteAsync (int id, string webRootPath);
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

        public async Task DeleteAsync (int id, string webRootPath)
        {
            var serviceOrder = await _context.ServiceOrders
                .Include(so => so.DecorPrints)
                .FirstOrDefaultAsync(so => so.Id == id);

            if (serviceOrder != null)
            {
                // Deletar as imagens físicas antes de remover do banco
                if (serviceOrder.DecorPrints != null && serviceOrder.DecorPrints.Any())
                {
                    foreach (var decorPrint in serviceOrder.DecorPrints)
                    {
                        if (!string.IsNullOrEmpty(decorPrint.ImageUrl))
                        {
                            DeleteImageFile(decorPrint.ImageUrl, webRootPath);
                        }
                    }

                    _context.DecorPrints.RemoveRange(serviceOrder.DecorPrints);
                }

                _context.ServiceOrders.Remove(serviceOrder);
                await _context.SaveChangesAsync();
            }
        }

        private void DeleteImageFile (string fileName, string webRootPath)
        {
            try
            {
                string filePath = Path.Combine(webRootPath, "images", "uploads", fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao deletar imagem {fileName}: {ex.Message}");
            }
        }
    }
}
