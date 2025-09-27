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

        [Display(Name = "Link da Imagem")]
        public string? ImageUrl { get; set; } 

        [Display(Name = "Tipo de Papel")]
        public int PaperId { get; set; }

        [Display(Name = "Tipo de Vidro")]
        public int GlassTypeId { get; set; }

        [Display(Name = "Tipo de Moldura")]
        public int FrameId { get; set; }

        [Display(Name = "Tipo de Fundo")]
        public int BackingId { get; set; }

        public IEnumerable<SelectListItem>? PaperOptions { get; set; }
        public IEnumerable<SelectListItem>? GlassTypeOptions { get; set; }
        public IEnumerable<SelectListItem>? FrameOptions { get; set; }
        public IEnumerable<SelectListItem>? BackingOptions { get; set; }
    }
}