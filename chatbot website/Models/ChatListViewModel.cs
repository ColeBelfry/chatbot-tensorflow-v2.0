using DAL.Models;

namespace chatbot_website.Models
{
	public class ChatListViewModel
	{
		public List<(string userInput, string botResponse)> ChatPairs { get; set; }
		public List<ChatBot> Bots { get; set; }

		public ChatListViewModel()
		{
			this.ChatPairs = new List<(string userInput, string botResponse)>();

			this.Bots = new List<ChatBot>();

		}
		
	}
}
