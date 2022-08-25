namespace chatbot_website.Models
{
	public class ChatListViewModel
	{
		public List<(string userInput, string botResponse)> ChatPairs = new List<(string userInput, string botResponse)> ();
	}
}
