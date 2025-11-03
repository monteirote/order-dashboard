using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Database.Entities;
using OrderDashboard.Database.Entities.ENUMs;
using OrderDashboard.DTOs;
using OrderDashboard.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace OrderDashboard.Repositories
{
    public interface IDecorPrintsRepository
    {
        void AddDecorPrint (DecorPrintDTO viewModel);
        List<DashboardOSViewModel> GetDashboardData ();
        Task DeleteDecorPrintAsync(int id, string webRootPath);
    }

    public class DecorPrintsRepository : IDecorPrintsRepository
    {
        private readonly MainContext _context;

        public DecorPrintsRepository (MainContext context)
        {
            _context = context;
        }

        public void AddDecorPrint (DecorPrintDTO viewModel)
        {
            var decorPrint = new DecorPrints
            {
                Height = viewModel.Height,
                Width = viewModel.Width,
                ImageUrl = viewModel.ImageUrl,
                GlassTypeId = viewModel.GlassTypeId,
                FrameId = viewModel.FrameId,
                Description = viewModel.Description,
                ServiceOrderId = viewModel.ServiceOrderId
            };

            _context.DecorPrints.Add(decorPrint);
            _context.SaveChanges();
        }

        public List<DashboardOSViewModel> GetDashboardData ()
        {
            var decorPrints = _context.DecorPrints
                .Include(x => x.ServiceOrder)
                .Where(x => x.ServiceOrder.Status == ServiceOrderStatus.InProgress)
                .Include(dp => dp.Frame)
                .Include(dp => dp.GlassType);

            var groupedServiceOrders = decorPrints
                .GroupBy(dp => dp.ServiceOrder)
                .Select(grupo => new DashboardOSViewModel {
                    OSNumber = grupo.Key.Number,
                    CustomerName = grupo.Key.CustomerName ?? "",
                    DueDate = grupo.Key.DueDate,
                    Quadros = grupo.Select(quadro => new DashboardQuadroViewModel {
                        ImageUrl = quadro.ImageUrl ?? "",
                        Height = quadro.Height,
                        Width = quadro.Width,
                        FrameTypeName = quadro.Frame != null ? quadro.Frame.Name : "",
                        GlassTypeName = quadro.GlassType != null ? quadro.GlassType.Name : "",
                        Description = quadro.Description ?? ""
                    }).ToList()
                })
                .OrderBy(os => os.DueDate)
                .ToList();

            return groupedServiceOrders;
        }

        public async Task DeleteDecorPrintAsync(int id, string webRootPath)
        {
            var decorPrint = await _context.DecorPrints.FindAsync(id);

            if (decorPrint != null)
            {
                // Deletar a imagem física se existir
                if (!string.IsNullOrEmpty(decorPrint.ImageUrl))
                {
                    DeleteImageFile(decorPrint.ImageUrl, webRootPath);
                }

                _context.DecorPrints.Remove(decorPrint);
                await _context.SaveChangesAsync();
            }
        }

        private void DeleteImageFile(string fileName, string webRootPath)
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
                // Log o erro, mas não falha a operação de exclusão
                Console.WriteLine($"Erro ao deletar imagem {fileName}: {ex.Message}");
            }
        }
    }
}
