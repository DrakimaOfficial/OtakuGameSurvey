using Microsoft.AspNetCore.Mvc;

namespace CoreMvc.Controllers.Mvc
{
    public class SurveyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }
    }
}