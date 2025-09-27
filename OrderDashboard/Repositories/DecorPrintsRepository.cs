using OrderDashboard.Database;
using OrderDashboard.ViewModels;

namespace OrderDashboard.Repositories
{
    public interface IDecorPrintsRepository
    {
        void AddDecorPrint (DecorPrintsViewModel viewModel);
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
    }
}
