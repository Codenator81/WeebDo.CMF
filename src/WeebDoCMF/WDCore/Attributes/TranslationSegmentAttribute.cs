using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Globalization;
using WeebDoCMF.WDCore.Models.Translations;

namespace WeebDoCMF.WDCore.Attributes
{
    /// <summary>
    /// Get culture segment from route and add transltions
    /// </summary>
    /// 
    public class TranslationSegmentAttribute : TypeFilterAttribute
    {
        public TranslationSegmentAttribute() : base(typeof(TranslationSegmentAttributeImpl)) { }
    }

    public class TranslationSegmentAttributeImpl : ActionFilterAttribute
    {

        private readonly ITRepository _tRepository;

        public TranslationSegmentAttributeImpl(ITRepository tRepository)
        {
            _tRepository = tRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string cultureFromRoute = context.RouteData.Values["culture"] as string;
            var cultureCode = _tRepository.GetCultureByCode(cultureFromRoute);

            if (!string.IsNullOrEmpty(cultureCode))
            {
                var culture = new CultureInfo(cultureCode);
                //set culture
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
        }
    }
}
