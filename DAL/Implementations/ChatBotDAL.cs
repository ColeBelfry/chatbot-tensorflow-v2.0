using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementations
{
	public class ChatBotDAL : IChatBotDAL
	{
		ChatBotContext context;

		public ChatBotDAL(ChatBotContext context)
		{
			this.context = context;
		}

		public void AddChatBot(ChatBot chatBot)
		{
			context.Bots.Add(chatBot);
		}

		public List<ChatBot> GetAllChatBots()
		{
			var chatbots = context.Bots.ToList();
			return chatbots;
		}

		public ChatBot GetChatBotById(int id)
		{
			var bot = context.Bots.Where(b => b.Id == id).First();
			return bot;
		}

		public ChatBot GetChatBotByName(string name)
		{
			var bot = context.Bots.Where(b => b.Name == name).First();
			return bot;
		}

		public void RemoveChatBot(int id)
		{
			var bot = context.Bots.Where(b => b.Id == id).First();
			context.Bots.Remove(bot);
			context.SaveChanges();
		}
	}
}
