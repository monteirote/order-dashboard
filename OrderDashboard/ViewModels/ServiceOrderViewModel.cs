using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrderDashboard.ViewModels
{
    public class ServiceOrderViewModel
    {
        [Required(ErrorMessage = "O número da OS é obrigatório.")]
        [Display(Name = "Número da OS")]
        public string OSNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [Display(Name = "Nome do Cliente")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de entrega é obrigatória.")]
        [Display(Name = "Data e Hora de Entrega")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; } = DateTime.Now;

        public List<DecorPrintsViewModel> DecorPrints { get; set; } = new List<DecorPrintsViewModel>();

        public IEnumerable<SelectListItem> GlassTypeOptions { get; set; } = [];
        public IEnumerable<SelectListItem> FrameOptions { get; set; } = [];

    }
}
