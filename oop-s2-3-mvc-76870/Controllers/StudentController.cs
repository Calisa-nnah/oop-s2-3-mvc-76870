using Microsoft.AspNetCore.Mvc;

namespace VgcCollege.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
