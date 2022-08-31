using chatbot_website.Models;
using Microsoft.AspNetCore.Mvc;
using PythonInterpreter;

namespace chatbot_website.Controllers
{
    public class ChatController : Controller
    {
        ToPython interpreter = new ToPython();
        static ChatListViewModel chatModel = new ChatListViewModel();
        public IActionResult ChatWindow()
        {
            return View(chatModel);
        }

		public IActionResult ChangeModel(string id)
		{
            currentBot = id;
            chatModel.CurrentBot = id;
			return View("ChatWindow",chatModel);
		}

		[HttpPost]
        public IActionResult ChatWindow(string usermsg)
        {
            string botRes = interpreter.ExecuteChatFunction("bob", usermsg);
            chatModel.ChatPairs.Add((usermsg, botRes));
            return View(chatModel);
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
