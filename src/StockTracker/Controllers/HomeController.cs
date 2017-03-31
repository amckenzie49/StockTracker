using Microsoft.AspNetCore.Mvc;
using StockTracker.Entities;
using StockTracker.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
using StockTracker.Services;
using System.Threading.Tasks;

namespace StockTracker.Controllers
{
    public class HomeController : Controller
    {

        private IStockData _stockService;

        public HomeController(IStockData stockService)
        {
            _stockService = stockService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel inputModel)
        {

            Stock workingStock = new Stock();

            workingStock.TickerSymbol = inputModel.TickerSymbol;

            await _stockService.getStockData(workingStock);

            return View("Details", workingStock);
        }
    }
}
