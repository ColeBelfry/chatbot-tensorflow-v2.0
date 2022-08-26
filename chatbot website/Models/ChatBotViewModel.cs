﻿namespace chatbot_website.Models
{
    public class ChatBotViewModel
    {
        public string Name { get; set; }
        public int Epochs { get; set; }
        public int BatchSize { get; set; }
        public int LearningRate { get; set; }
        public List<(string, int)> HiddenLayers { get; set; }

        public ChatBotViewModel()
        {
            HiddenLayers = new List<(string, int)>();
        }
    }
}
