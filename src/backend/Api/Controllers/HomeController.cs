using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, Route("/")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}