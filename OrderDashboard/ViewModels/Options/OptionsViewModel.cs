using OrderDashboard.Database.Entities;

namespace OrderDashboard.ViewModels.Options
{
    public class OptionsManagementViewModel
    {
        public IEnumerable<GlassTypes> GlassTypes { get; set; }
        public IEnumerable<Frames> FrameTypes { get; set; }
    }
}
