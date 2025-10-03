using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Database.Entities;
using OrderDashboard.DTOs;
using OrderDashboard.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace OrderDashboard.Repositories
{
    public interface IDecorPrintsRepository
    {
        void AddDecorPrint (DecorPrintDTO viewModel);
        List<DecorPrintDetailsViewModel> GetAllDecorPrints ();
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

        public List<DecorPrintDetailsViewModel> GetAllDecorPrints ()
        {
            var itens = _context.DecorPrints.Include(d => d.GlassType).Include(d => d.Frame).ToList();

            return itens.Select(d => new DecorPrintDetailsViewModel {
                Height = d.Height,
                Width = d.Width,
                ImageUrl = d.ImageUrl,
                GlassTypeName = d.GlassType?.Name,
                FrameTypeName = d.Frame?.Name,
                Description = d.Description

            }).ToList();
        }
    }
}
