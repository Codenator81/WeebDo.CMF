using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.Localization;
using System.Globalization;
using WeebDoCMF.WDCore.Models.Translations;
using System.Threading;

namespace WeebDoCMF.WDCore.Controllers
{
    public class WDTranslationController : Controller
    {
        [FromServices]
        public ITRepository tRepository { get; set; }

        public IHtmlLocalizer<WDTranslationController> SR;

        protected WDTranslationController(IHtmlLocalizer<WDTranslationController> localizer)
            : base()
        {
            SR = localizer;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string cultureFromRoute = RouteData.Values["culture"] as string;
            var cultureCode = tRepository.GetCultureByCode(cultureFromRoute);
            
            if (!string.IsNullOrEmpty(cultureCode))
            {
                var culture = new CultureInfo(cultureCode);
#if DNX451
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
#else
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
#endif
            }
            base.OnActionExecuting(context);
        }
    }
}
