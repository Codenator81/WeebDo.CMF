using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using WeebDoCMF.Settings;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WeebDoCMF.Areas.WDAdmin.Controllers
{
    [Area("WDAdmin")]
    public class AdminController : Controller
    {
        private IOptions<AppSettings> _appSettings;

        public AdminController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            string appName = _appSettings.Value.SiteName;
            ViewBag.siteName = appName;
            return View();
        }
    }
}
