using Microsoft.AspNetCore.Mvc;

namespace MeetingApp.Controllers
{
    public class ErrorController:Controller 
    {
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult RegisterError()
        {
            return View();
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

    }
}