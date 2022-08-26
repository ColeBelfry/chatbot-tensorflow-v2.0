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
			throw new NotImplementedException();
		}

		public List<ChatBot> GetAllChatBots()
		{
			var chatbots = context.Bots.ToList();
			return chatbots;
		}

		public ChatBot GetChatBotById(int id)
		{
			throw new NotImplementedException();
		}

		public ChatBot GetChatBotByName(string name)
		{
			throw new NotImplementedException();
		}

		public void RemoveChatBot(ChatBot chatBot)
		{
			throw new NotImplementedException();
		}
	}
}
