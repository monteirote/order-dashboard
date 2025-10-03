using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OrderDashboard.DTOs;
using OrderDashboard.Repositories;
using OrderDashboard.ViewModels;

namespace OrderDashboard.Controllers
{
    public class ServiceOrderController : Controller
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly IDecorPrintsRepository _decorPrintRepository;
        private readonly IOptionsRepository _optionsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceOrderController (IServiceOrderRepository serviceOrderRepository, IOptionsRepository optionsRepository, IWebHostEnvironment webHostEnvironment, IDecorPrintsRepository decorPrintsRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _optionsRepository = optionsRepository;
            _webHostEnvironment = webHostEnvironment;
            _decorPrintRepository = decorPrintsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index ()
        {
            var serviceOrders = await _serviceOrderRepository.GetAllAsync();

            var viewModel = serviceOrders.Select(so => new ServiceOrderListViewModel
            {
                Id = so.Id,
                OSNumber = so.Number,
                CustomerName = so.CustomerName,
                DueDate = so.DueDate,
                FramesCount = so.DecorPrints.Count
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details (int id)
        {
            var serviceOrder = await _serviceOrderRepository.GetByIdWithDetailsAsync(id);

            if (serviceOrder == null)
            {
                return NotFound();
            }

            var viewModel = new ServiceOrderDetailViewModel
            {
                Id = serviceOrder.Id,
                OSNumber = serviceOrder.Number,
                CustomerName = serviceOrder.CustomerName ?? "",
                DueDate = serviceOrder.DueDate,
                DecorPrints = serviceOrder.DecorPrints.Select(dp => new DecorPrintDetailsViewModel
                {
                    Height = dp.Height,
                    Width = dp.Width,
                    Description = dp.Description ?? "",
                    ImageUrl = dp.ImageUrl ?? "",
                    GlassTypeName = dp.GlassType?.Name,
                    FrameTypeName = dp.Frame?.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ServiceOrderViewModel model)
        {

            if (!ModelState.IsValid)
            {
                model.GlassTypeOptions = _optionsRepository.GetGlassTypeOptions();
                model.FrameOptions = _optionsRepository.GetFrameOptions();

                return View("Index", model);
            }

            var serviceOrder = await _serviceOrderRepository.Add(model);

            if (serviceOrder == null)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao criar a ordem de serviço. Tente novamente.");
                model.GlassTypeOptions = _optionsRepository.GetGlassTypeOptions();
                model.FrameOptions = _optionsRepository.GetFrameOptions();
                return View("Index", model);
            }

            foreach (var quadro in model.DecorPrints)
            {
                string? uniqueFileName = null;

                if (quadro.ImageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + quadro.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await quadro.ImageFile.CopyToAsync(fileStream);
                    }
                }

                var decorPrint = new DecorPrintDTO
                {
                    Height = quadro.Height,
                    Width = quadro.Width,
                    ImageUrl = uniqueFileName,
                    GlassTypeId = quadro.GlassTypeId,
                    FrameId = quadro.FrameId,
                    Description = quadro.Description,
                    ServiceOrderId = serviceOrder.Id
                };

                _decorPrintRepository.AddDecorPrint(decorPrint);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ServiceOrderForm()
        {
            var viewModel = new ServiceOrderViewModel
            {
                GlassTypeOptions = _optionsRepository.GetGlassTypeOptions(),
                FrameOptions = _optionsRepository.GetFrameOptions()
            };

            return View(viewModel);
        }
    }
}