namespace Company.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [Route("/Error/{statusCode}")]
        public IActionResult HttpInvalidStatusCodeHandler(int statusCode)
        {
            return this.View("NotFound");
        }
    }
}
