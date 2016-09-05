using System.Web.Mvc;
using ConvictionCheck.Web.Models;

namespace ConvictionCheck.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("СonvictionCheck");
        }

        [HttpPost]
        public ActionResult AddNewCondiction(ConvictionDto conviction)
        {
            if (!ModelState.IsValid)
            {
                return View("СonvictionCheck", conviction);
            }

            return RedirectToAction("Index");
        }
    }
}