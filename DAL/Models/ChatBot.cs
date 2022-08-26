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
		public List<(string, int)> HiddenLayers { get; set; }
	}
}
