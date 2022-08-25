using Microsoft.AspNetCore.Mvc;

namespace chatbot_website.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult ChatWindow()
        {
            return View();
        }

        public IActionResult NewIntent()
        {
            return View();
        }

        public IActionResult NewModel()
        {
            return View();
        }
    }
}
