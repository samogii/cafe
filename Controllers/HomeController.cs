using Cafe.Data;
using Cafe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cafe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext _dataContext;


        public HomeController(ILogger<HomeController> logger , DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        public IActionResult Index(string? searchQuery)
        {
            var user = (User?)HttpContext.Items["User"];
            ViewBag.UserModel = user;
            var products = _dataContext.Products.ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery)).ToList();
            }
            return View("~/Views/Products/CardList.cshtml",  products);
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

        [Route("Home/404")]
        public IActionResult notFound()
        {
            return View("Error404");
        }
    }
}
