using Microsoft.AspNetCore.Mvc;
using OrderDashboard.Repositories;
using OrderDashboard.ViewModels.QrCodeInteraction;

namespace OrderDashboard.Controllers
{
    public class QRCodeInteractionController : Controller
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;

        public QRCodeInteractionController (IServiceOrderRepository serviceOrderRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
        }

        [HttpGet]
        [Route("QRCodeInteraction/CompleteFromQR/{osNumber}")]

        public async Task<IActionResult> CompleteFromQR (string osNumber)
        {
            var serviceOrder = await _serviceOrderRepository.GetByOsNumberWithDetailsAsync(osNumber);
            if (serviceOrder == null)
            {
                ViewBag.Message = "Ordem de Serviço não encontrada ou já processada.";
                return View("ActionStatus");
            }

            var viewModel = new CompleteFromQRViewModel
            {
                Id = serviceOrder.Id,
                OSNumber = serviceOrder.Number,
                CustomerName = serviceOrder.CustomerName ?? "",
                DueDate = serviceOrder.DueDate
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteFromQR(int id, IFormCollection form)
        {
            await _serviceOrderRepository.MarkAsCompleteAsync(id);

            ViewBag.Message = "Ordem de Serviço marcada como concluída com sucesso!";
            return View("ActionStatus");
        }
    }
}
