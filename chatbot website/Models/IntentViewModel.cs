using DAL.Models;

namespace chatbot_website.Models
{
    public class IntentViewModel
    {
        public string IntentName { get; set; }
        public List<string> Patterns { get; set; }
        public List<string> Responses { get; set; }

        public IntentViewModel()
        {
            this.Patterns = new List<string>();
            this.Responses = new List<string>();
        }
    }
}
