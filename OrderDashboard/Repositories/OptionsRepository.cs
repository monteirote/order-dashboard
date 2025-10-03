using Microsoft.AspNetCore.Mvc.Rendering;
using OrderDashboard.Database;

namespace OrderDashboard.Repositories
{
    public interface IOptionsRepository
    {
        List<SelectListItem> GetGlassTypeOptions();
        List<SelectListItem> GetFrameOptions();
    }

    public class OptionsRepository : IOptionsRepository
    {

        private readonly MainContext _context;

        public OptionsRepository (MainContext context)
        {
            _context = context;
        }

        public List<SelectListItem> GetGlassTypeOptions()
        {
            return _context.GlassTypes
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
        }

        public List<SelectListItem> GetFrameOptions()
        {
            return _context.Frames
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
        }

    }
}
