using chatbot_website.Models;
using chatbot_website.Tools;
using DAL.Data;
using DAL.Implementations;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PythonInterpreter;

namespace chatbot_website.Controllers
{
    public class ChatController : Controller
    {
        ToPython interpreter = new ToPython();
        private readonly IChatBotDAL dal;
		static ChatListViewModel chatModel = new ChatListViewModel();
        static IntentViewModel intentModel = new IntentViewModel();
        static ChatBotViewModel chatBotModel = new ChatBotViewModel();
        JsonEditor json_editor = new JsonEditor();

        public ChatController(ChatBotContext context)
        {
            this.dal = new ChatBotDAL(context);
        }
        public IActionResult ChatWindow()
        {
            return View(chatModel);
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
            return View(intentModel);
        }

        [HttpPost]
		public IActionResult NewIntent(string intent_name)
		{
            intentModel.IntentName = intent_name;
            var newIntent = new Intent() { intent = intent_name, patterns = intentModel.Patterns, responses = intentModel.Responses};
            //Put the actuall working file path here for Intent.json
            json_editor.AddToJson("Intent.json", newIntent);
            //Call a method to retrain the models
			return View("ChatWindow", chatModel);
		}

        [HttpPost]
        public IActionResult NewPattern(string pattern)
        {
            intentModel.Patterns.Add(pattern);
            return View("NewIntent", intentModel);
        }

		[HttpPost]
		public IActionResult NewResponse(string response)
		{
			intentModel.Responses.Add(response);
			return View("NewIntent", intentModel);
		}

		public IActionResult NewModel()
        {
            return View();
        }
        [HttpPost]
		public IActionResult NewModel(string name, int epochs, int batch_size, double learning_rate)
		{
            var newChatBot = new ChatBot() { Name = name, Epochs = epochs, BatchSize = batch_size, LearingRate = learning_rate, HiddenLayers = chatBotModel.HiddenLayers};
            dal.AddChatBot(newChatBot);
            //Call a method to train the model
			return View("ChatWindow", chatBotModel);
		}
	}
}
