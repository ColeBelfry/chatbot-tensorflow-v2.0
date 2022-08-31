using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ChatBot
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Epochs { get; set; }
		public int BatchSize { get; set; }
		public double LearingRate { get; set; }
		public List<HiddenLayer> HiddenLayers { get; set; }

		public ChatBot() { }
		public ChatBot(string name, int epochs, int batchSize, double learingRate, List<HiddenLayer> hiddenLayers)
		{
			Name = name;
			Epochs = epochs;
			BatchSize = batchSize;
			LearingRate = learingRate;
			HiddenLayers = hiddenLayers;
		}
	}
}
