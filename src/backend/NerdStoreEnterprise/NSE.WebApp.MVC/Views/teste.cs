using Microsoft.AspNetCore.Mvc;

namespace NSE.WebApp.MVC.Views
{
    public class teste : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}