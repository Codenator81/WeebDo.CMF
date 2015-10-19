using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WeebDoCMF.WDCore.Models.Translations;
using Microsoft.AspNet.Mvc.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WeebDoCMF.Controllers
{
    public class HomeController : Controller
    {
        private IHtmlLocalizer<HomeController> SR;

        public HomeController(IHtmlLocalizer<HomeController> localizer)
        {
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
