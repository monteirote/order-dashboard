using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrderDashboard.ViewModels
{
    public class DecorPrintsViewModel
    {
        [Required(ErrorMessage = "A altura é obrigatória.")]
        [Display(Name = "Altura (cm)")]
        public int Height { get; set; }

        [Required(ErrorMessage = "A largura é obrigatória.")]
        [Display(Name = "Largura (cm)")]
        public int Width { get; set; }

        [Display(Name = "Tipo de Vidro")]
        public int? GlassTypeId { get; set; }

        [Display(Name = "Tipo de Moldura")]
        public int? FrameId { get; set; }

        [Display(Name = "Imagem do Quadro")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Observações")]
        public string? Description { get; set; } = string.Empty;
    }
}