using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Noticias.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        
    }
}
