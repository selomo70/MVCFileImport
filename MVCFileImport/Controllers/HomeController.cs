using Microsoft.AspNetCore.Mvc;
using MVCFileImport.Models;
using System.Diagnostics;

namespace MVCFileImport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var csvFilePath = Path.Combine(_environment.WebRootPath, "data", "data.csv");

                List<OutputModel> nameCounts = new List<OutputModel>();
                List<OutputModel2> cityList = new List<OutputModel2>();

                if (System.IO.File.Exists(csvFilePath))
                {
                    (nameCounts, cityList) = Processor.ProcessCsv(csvFilePath);
                }

                ViewBag.NameCounts = nameCounts;
                ViewBag.CityList = cityList;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError("No file uploaded"?.ToString(), ex.InnerException);
                throw;
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
