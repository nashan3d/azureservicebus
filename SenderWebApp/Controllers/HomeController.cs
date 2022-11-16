using Microsoft.AspNetCore.Mvc;
using SampleShared.Models;
using SenderWebApp.Models;
using SenderWebApp.Services;
using System.Diagnostics;

namespace SenderWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAzureBusService _azureBusService;


        public HomeController(ILogger<HomeController> logger, IAzureBusService azureBusService)
        {
            _logger = logger;
            _azureBusService = azureBusService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Person person)
        {
            await _azureBusService.SendMessageAsync(person, "personque");
            return RedirectToAction("Privacy");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}