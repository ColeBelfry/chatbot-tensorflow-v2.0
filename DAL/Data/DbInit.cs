using DAL.Models;
using PythonInterpreter;
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

			SetupChatBots(context);
			SetupLayers(context);
		}

		public static void SetupChatBots(ChatBotContext context)
		{
			if (context.Bots.Any())
			{
				return;
			}
			ToPython interpreter = new ToPython();
			var ChatBots = new ChatBot[]
			{
				new ChatBot()
				{
					Name = "default",
					Epochs = 1000,
					BatchSize = 500,
					LearingRate = 0.001,
				},
				new ChatBot()
				{
					Name = "bob",
					Epochs = 900,
					BatchSize = 50,
					LearingRate = 0.001,
				}
			};

			foreach(var bot in ChatBots)
			{
				//var types = new List<string>();
				//var vals = new List<int>();
				//foreach(var hiddenLayer in bot.HiddenLayers)
				//{
				//	types.Add(hiddenLayer.LayerType);
				//	vals.Add(hiddenLayer.LayerValue);
				//}
				context.Bots.Add(bot);
				//interpreter.ExecuteCreateModelFunction(bot.Name, bot.Epochs, bot.BatchSize, bot.LearingRate, types.ToArray(), vals.ToArray());
			}
			context.SaveChanges();
		}

        public static void SetupLayers(ChatBotContext context)
		{
            if (context.HiddenLayers.Any())
            {
                return;
            }

			HiddenLayer[] hiddenLayers = new HiddenLayer[]
			{
                new HiddenLayer()
                {
					ChatbotId = context.Bots.AsEnumerable().ElementAt(0).Id,
                    LayerType = "dense",
                    LayerValue = 8
                },
                new HiddenLayer()
                {
                    ChatbotId = context.Bots.AsEnumerable().ElementAt(0).Id,
                    LayerType = "dense",
                    LayerValue = 8
                },
                new HiddenLayer()
                {
                    ChatbotId = context.Bots.AsEnumerable().ElementAt(0).Id,
                    LayerType = "dense",
                    LayerValue = 8
                },
                new HiddenLayer()
                {
                    ChatbotId = context.Bots.AsEnumerable().ElementAt(1).Id,
                    LayerType = "dense",
                    LayerValue = 8
                },
                new HiddenLayer()
                {
                    ChatbotId = context.Bots.AsEnumerable().ElementAt(1).Id,
                    LayerType = "dense",
                    LayerValue = 8
                },
                new HiddenLayer()
                {
                    ChatbotId = context.Bots.AsEnumerable().ElementAt(1).Id,
                    LayerType = "dense",
                    LayerValue = 8
                }
            };

            context.HiddenLayers.AddRange(hiddenLayers);
            context.SaveChanges();
        }
    }
}
