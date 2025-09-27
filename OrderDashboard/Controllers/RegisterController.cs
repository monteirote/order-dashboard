using Microsoft.AspNetCore.Mvc;
using OrderDashboard.Repositories;
using OrderDashboard.ViewModels;
using System.Diagnostics;

namespace OrderDashboard.Controllers
{
    public class RegisterController : Controller
    {

        private readonly IOptionsRepository _optionsRepository;
        private readonly IDecorPrintsRepository _decorPrintsRepository;

        public RegisterController (IOptionsRepository optionsRepository, IDecorPrintsRepository decorPrintsRepository)
        {
            _optionsRepository = optionsRepository;
            _decorPrintsRepository = decorPrintsRepository;
        }

        public IActionResult Index()
        {
            var viewModel = new DecorPrintsViewModel
            {
                PaperOptions = _optionsRepository.GetPaperOptions(),
                GlassTypeOptions = _optionsRepository.GetGlassTypeOptions(),
                FrameOptions = _optionsRepository.GetFrameOptions(),
                BackingOptions = _optionsRepository.GetBackingOptions()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDecorPrint (DecorPrintsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _decorPrintsRepository.AddDecorPrint(viewModel);

                    TempData["SuccessMessage"] = "Quadro cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao salvar o quadro. Verifique os dados e tente novamente. Erro: " + ex.Message);
                }
            }

            viewModel.PaperOptions = _optionsRepository.GetPaperOptions();
            viewModel.GlassTypeOptions = _optionsRepository.GetGlassTypeOptions();
            viewModel.FrameOptions = _optionsRepository.GetFrameOptions();
            viewModel.BackingOptions = _optionsRepository.GetBackingOptions();

            return View(viewModel);
        }
    }
}
