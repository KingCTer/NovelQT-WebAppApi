using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NovelQT.Services.Api.Controllers
{
    public class BookController : PublicApiController
    {
        [HttpGet]
        public ActionResult<string> TestApi()
        {
            return "Ok";
        }
    }
}
