using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDashboard.Database.Entities;
using OrderDashboard.Repositories;

namespace OrderDashboard.Controllers
{
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly IOptionsRepository _optionsRepository;

        public OptionsController(IOptionsRepository optionsRepository)
        {
            _optionsRepository = optionsRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            ViewBag.GlassTypes = await _optionsRepository.GetAllGlassTypesAsync();
            ViewBag.FrameTypes = await _optionsRepository.GetAllFrameTypesAsync();
            return View();
        }

        [HttpGet]
        public IActionResult CreateGlassType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGlassType (GlassTypes glassType)
        {
            if (ModelState.IsValid)
            {
                await _optionsRepository.AddGlassTypeAsync(glassType);
                return RedirectToAction(nameof(Index));
            }
            return View(glassType);
        }

        [HttpGet]
        public IActionResult CreateFrameType()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFrameType (Frames frameType)
        {
            if (ModelState.IsValid)
            {
                await _optionsRepository.AddFrameTypeAsync(frameType);
                return RedirectToAction(nameof(Index));
            }
            return View(frameType);
        }
    }
}