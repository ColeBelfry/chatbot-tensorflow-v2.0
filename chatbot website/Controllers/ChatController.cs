using chatbot_website.Models;
using chatbot_website.Tools;
using DAL.Data;
using DAL.Implementations;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using PythonInterpreter;
using static IronPython.Modules._ast;

namespace chatbot_website.Controllers
{
    public class ChatController : Controller
    {
        ToPython interpreter = new ToPython();
        private readonly IChatBotDAL dal;
		static ChatListViewModel chatModel = new ChatListViewModel();
        static IntentViewModel intentModel = new IntentViewModel();
        static ChatBotViewModel chatBotModel = new ChatBotViewModel();
        static string currentBot;
        JsonEditor json_editor = new JsonEditor();

        public ChatController(ChatBotContext context)
        {
            this.dal = new ChatBotDAL(context);
        }
        public IActionResult ChatWindow()
        {
            currentBot = "bob";
            chatModel.CurrentBot = "bob";
            chatModel.Bots = dal.GetAllChatBots();
            return View(chatModel);
        }


        public IActionResult ChangeModel(string id)
        {
            chatModel.ChatPairs.Clear();
            currentBot = id;
            chatModel.CurrentBot = id;
            return View("ChatWindow", chatModel);
        }





        [HttpPost]
        public IActionResult ChatWindow(string usermsg)
        {
            string botResPy = interpreter.ExecuteChatFunction(currentBot, usermsg);
            //the following portion chops of the default info for the bot response and properly builds a string to return
            botResPy = botResPy.Replace("\r\n", " ");
            string[] resSplit = botResPy.Split(' ');
			string botRes = "";
			for (int i = 0; i < resSplit.Length; i++)
            {
                if(i >= 11)
                {
                    botRes += resSplit[i] + " ";
                }
                
            }
            botRes = botRes.TrimEnd();
            
            chatModel.ChatPairs.Add((usermsg, botRes));
            return View(chatModel);
        }

        public IActionResult NewIntent()
        {
            return View(intentModel);
        }

        public IActionResult RemoveModel()
        {
            return View(chatModel);
        }
        

        [HttpPost]
		public IActionResult NewIntent(string intent_name)
		{
            intentModel.IntentName = intent_name;
            var newIntent = new Intent() { intent = intent_name, patterns = intentModel.Patterns, responses = intentModel.Responses};
            //Put the actual working file path here for Intent.json
            var json_path = Directory.GetParent(Directory.GetCurrentDirectory()) + "/Intent.json";
            var intentsList = json_editor.GetJsonListItents(json_path);
            foreach(var intent in intentsList)
            {
                if(intent.intent == intent_name)
                {
                    //if intent already exists
                    return View("NewIntent", intentModel);
                }
            }
            json_editor.AddToJson(json_path, newIntent);
            var bots = dal.GetAllChatBots();
            foreach(var bot in bots)
            {
				interpreter.ExecuteTrainModelFunction(bot.Name, bot.Epochs, bot.BatchSize, bot.LearingRate);
			}
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

        [HttpPost]
        public IActionResult NewLayer(string layer_type, string layer_value)
        {
            var newLayer = new HiddenLayer(layer_type, int.Parse(layer_value));
            chatBotModel.HiddenLayers.Add(newLayer);
            return View("NewModel", chatBotModel);
        }

		public IActionResult NewModel()
        {
            return View(chatBotModel);
        }
        [HttpPost]
		public IActionResult NewModel(string name, int epochs, int batch_size, double learning_rate)
		{
            var newChatBot = new ChatBot() { Name = name, Epochs = epochs, BatchSize = batch_size, LearingRate = learning_rate, HiddenLayers = chatBotModel.HiddenLayers};
            dal.AddChatBot(newChatBot);
            var type = new List<string>();
            var val = new List<int>();
            foreach(var layer in chatBotModel.HiddenLayers)
            {
                type.Add(layer.LayerType);
                val.Add(layer.LayerValue);
            }
            interpreter.ExecuteCreateModelFunction(newChatBot.Name, newChatBot.Epochs, newChatBot.BatchSize, newChatBot.LearingRate, type.ToArray(), val.ToArray());
            chatModel.Bots.Add(newChatBot);
            currentBot = newChatBot.Name;
            chatModel.CurrentBot = currentBot;
			return View("ChatWindow", chatModel);
		}
        [HttpPost]
        public IActionResult RemoveModel(string botname)
        {
            dal.RemoveChatBot(botname);
            chatModel.Bots = dal.GetAllChatBots();
            return View("ChatWindow", chatModel);
		}


        
	}
}
