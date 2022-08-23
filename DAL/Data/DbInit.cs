using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
	public class DbInit
	{
		public static void Initialize(ChatBotContext context)
		{
			context.Database.EnsureCreated();
		}

		public static void SetupChatBots(ChatBotContext context)
		{
			if (context.Bots.Any())
			{
				return;
			}

			var ChatBots = new ChatBot[]
			{
				new ChatBot()
				{
					Name = "default",
					Epochs = 1000,
					BatchSize = 500,
					LearingRate = 0.001,
					HiddenLayers = new List<(string, int)>
					{
						("dense", 8),
						("dense", 8),
						("dense", 8)
					}

				},

				new ChatBot()
				{
					Name = "John",
					Epochs = 100,
					BatchSize = 25,
					LearingRate = 0.001,
					HiddenLayers = new List<(string, int)>
					{
						("dense", 8),
						("dense", 8),
						("dense", 8)
					}
				}
			};

			foreach(var bot in ChatBots)
			{
				context.Bots.Add(bot);
			}
			context.SaveChanges();
		}
	}
}
