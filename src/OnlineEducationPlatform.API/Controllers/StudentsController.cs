using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationPlatform.API.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
