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
    }
}
