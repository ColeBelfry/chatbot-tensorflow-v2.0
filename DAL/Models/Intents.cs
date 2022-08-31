using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Intents
	{
		public List<Intent> intents { get; set; }
		public Intents() { }
		public Intents(List<Intent> intents)
		{
			this.intents = intents;
		}
	}
}
