using System.Collections.Generic;
using Microsoft.AspNet.Mvc;

namespace WeebDoCMF.WDApi.Controllers
{
    [Route("api/[controller]")]
    public class TranslationsController : Controller
    {
        // GET: api/translations
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/translations/ru/language
        [HttpGet("{culture}/{name}")]
        public JsonResult Get(string culture, string name)
        {
            return new JsonResult("{\"name\":\"" + name + "\", " + "\"culture\":\"" + culture + "\"}");
        }

        // POST api/translations
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/translations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/translations/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
