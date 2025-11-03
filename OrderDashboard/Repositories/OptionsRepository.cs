using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderDashboard.Database;
using OrderDashboard.Database.Entities;

namespace OrderDashboard.Repositories
{
    public interface IOptionsRepository
    {
        List<SelectListItem> GetGlassTypeOptions();
        List<SelectListItem> GetFrameOptions();


        Task<IEnumerable<GlassTypes>> GetAllGlassTypesAsync();
        Task<IEnumerable<Frames>> GetAllFrameTypesAsync();

        Task AddGlassTypeAsync (GlassTypes glassType);
        Task AddFrameTypeAsync (Frames frameType);
        
        Task DeleteGlassTypeAsync(int id);
        Task DeleteFrameTypeAsync(int id);
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

        public async Task<IEnumerable<GlassTypes>> GetAllGlassTypesAsync()
        {
            return await _context.GlassTypes.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<IEnumerable<Frames>> GetAllFrameTypesAsync()
        {
            return await _context.Frames.OrderBy(f => f.Name).ToListAsync();
        }

        public async Task AddGlassTypeAsync(GlassTypes glassType)
        {
            _context.GlassTypes.Add(glassType);
            await _context.SaveChangesAsync();
        }

        public async Task AddFrameTypeAsync(Frames frameType)
        {
            _context.Frames.Add(frameType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGlassTypeAsync(int id)
        {
            var glassType = await _context.GlassTypes.FindAsync(id);
            if (glassType != null)
            {
                _context.GlassTypes.Remove(glassType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteFrameTypeAsync(int id)
        {
            var frameType = await _context.Frames.FindAsync(id);
            if (frameType != null)
            {
                _context.Frames.Remove(frameType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
