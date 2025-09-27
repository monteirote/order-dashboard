using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.DTOs;
using OrderDashboard.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace OrderDashboard.Repositories
{
    public interface IDecorPrintsRepository
    {
        void AddDecorPrint (DecorPrintsViewModel viewModel);
        List<DecorPrintDTO> GetAllDecorPrints();
    }

    public class DecorPrintsRepository : IDecorPrintsRepository
    {
        private readonly MainContext _context;

        public DecorPrintsRepository (MainContext context)
        {
            _context = context;
        }

        public void AddDecorPrint (DecorPrintsViewModel viewModel)
        {
            var decorPrint = new Database.Entities.DecorPrints
            {
                Height = viewModel.Height,
                Width = viewModel.Width,
                ImageUrl = viewModel.ImageUrl,
                PaperId = viewModel.PaperId,
                GlassTypeId = viewModel.GlassTypeId,
                FrameId = viewModel.FrameId,
                BackingId = viewModel.BackingId
            };

            _context.DecorPrints.Add(decorPrint);
            _context.SaveChanges();
        }

        public List<DecorPrintDTO> GetAllDecorPrints()
        {
            var itens = _context.DecorPrints.Include(d => d.Paper).Include(d => d.GlassType)
                                            .Include(d => d.Frame).Include(d => d.Backing);

            return itens.Select(d => new DecorPrintDTO {
                Id = d.Id,
                Height = d.Height,
                Width = d.Width,
                ImageUrl = "/images/foto-placeholder.jpg",
                Paper = d.Paper != null ? d.Paper.Name : null,
                GlassType = d.GlassType != null ? d.GlassType.Name : null,
                Frame = d.Frame != null ? d.Frame.Name : null,
                Backing = d.Backing != null ? d.Backing.Name : null
            }).ToList();
        }
    }
}
