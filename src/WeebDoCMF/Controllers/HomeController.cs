using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Localization;
using WeebDoCMF.WDCore.Attributes;

namespace WeebDoCMF.Controllers
{
    [TranslationSegment]
    public class HomeController : Controller
    {
        IHtmlLocalizer<HomeController> SR;
        public HomeController(IHtmlLocalizer<HomeController> localizer) {
            SR = localizer;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Trans = SR["language"];            
            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public IActionResult StatusCodePage()
        {
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }
    }
}
