using Microsoft.AspNetCore.Mvc;
using OrderDashboard.Repositories;

namespace OrderDashboard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDecorPrintsRepository _decorPrintsRepository;

        public DashboardController (IDecorPrintsRepository decorPrintsRepository)
        {
            _decorPrintsRepository = decorPrintsRepository;
        }   


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ObterQuadrosEmProducao()
        {
            var quadros = _decorPrintsRepository.GetDashboardData();
            return Json(quadros);
        }
    }

}
