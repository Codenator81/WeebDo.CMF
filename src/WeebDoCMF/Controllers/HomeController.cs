using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using WeebDoCMF.WDCore.Models.Translations;
using Microsoft.AspNet.Mvc.Localization;
using WeebDoCMF.WDCore.Controllers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WeebDoCMF.Controllers
{
    public class HomeController : WDTranslationController
    {
        public HomeController(IHtmlLocalizer<WDTranslationController> localizer) :base(localizer) { }
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
