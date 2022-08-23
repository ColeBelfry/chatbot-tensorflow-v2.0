using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
	public interface IChatBotDAL
	{
		List<ChatBot> GetAllChatBots();
		ChatBot GetChatBotById(int id);
		ChatBot GetChatBotByName(string name);
		void AddChatBot(ChatBot chatBot);
		void RemoveChatBot(ChatBot chatBot);
	}
}
