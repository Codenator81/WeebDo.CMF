using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WeebDoCMF.WDAdmin.Controllers
{
    [Area("WDAdmin")]
    [Authorize("ManageAdminPanel")]
    public class AdminController : Controller
    {
        private AppSettings _appSettings;

        public AdminController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.SiteLogoName = _appSettings.SiteLogoName;
            return View();
        }       
    }
}
