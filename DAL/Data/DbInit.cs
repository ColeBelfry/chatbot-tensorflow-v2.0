﻿using DAL.Models;
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
					HiddenLayers = new List<HiddenLayer>
					{
						new HiddenLayer()
						{
							LayerType = "dense",
							LayerValue = 8
						},
                        new HiddenLayer()
                        {
                            LayerType = "dense",
                            LayerValue = 8
                        },
                        new HiddenLayer()
                        {
                            LayerType = "dense",
                            LayerValue = 8
                        }
                    }

				},

				new ChatBot()
				{
					Name = "John",
					Epochs = 100,
					BatchSize = 25,
					LearingRate = 0.001,
					HiddenLayers = new List<HiddenLayer>
					{
                        new HiddenLayer()
                        {
                            LayerType = "dense",
                            LayerValue = 8
                        },
                        new HiddenLayer()
                        {
                            LayerType = "dense",
                            LayerValue = 8
                        },
                        new HiddenLayer()
                        {
                            LayerType = "dense",
                            LayerValue = 8
                        }
                    }
				}
			};

			foreach(var bot in ChatBots)
			{
				var types = new List<string>();
				var vals = new List<int>();
				foreach(var hiddenLayer in bot.HiddenLayers)
				{
					types.Add(hiddenLayer.LayerType);
					vals.Add(hiddenLayer.LayerValue);
				}
				context.Bots.Add(bot);
				//interpreter.ExecuteCreateModelFunction(bot.Name, bot.Epochs, bot.BatchSize, bot.LearingRate, types.ToArray(), vals.ToArray());
			}
			context.SaveChanges();
		}
	}
}
