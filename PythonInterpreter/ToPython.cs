using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

namespace PythonInterpreter
{
	public class ToPython
	{
		private string solutionPath = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;

		public string ExecuteChatFunction(string modelName, string userInput)
		{
			try
			{
				ScriptEngine engine = Python.CreateEngine();
				ScriptScope scope = engine.CreateScope();
				engine.ExecuteFile(Path.Join(solutionPath, "\\chatbot-tensorflow-v2.0\\chatbot.py"), scope);
				dynamic function = scope.GetVariable("chat");
				return function(modelName, userInput);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return "Encountered error: " + ex;
			}
		}
		public void ExecuteCreateModelFunction(string modelName, int epochsNum, int batchSizeNum, int learningRateNum, (string, int) hiddenLayers)
		{
			try
			{
				ScriptEngine engine = Python.CreateEngine();
				ScriptScope scope = engine.CreateScope();
				engine.ExecuteFile(solutionPath + "\\chatbot-tensorflow-v2.0\\chatbot.py", scope);
				dynamic function = scope.GetVariable("createNewModel");
				function(modelName, epochsNum, batchSizeNum, learningRateNum, hiddenLayers);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public void ExecuteTrainModelFunction(string modelName, int epochsNum, int batchSizeNum, int learningRateNum)
		{
			try
			{
				ScriptEngine engine = Python.CreateEngine();
				ScriptScope scope = engine.CreateScope();
				engine.ExecuteFile(solutionPath + "\\chatbot-tensorflow-v2.0\\chatbot.py", scope);
				dynamic function = scope.GetVariable("train");
				function(modelName, epochsNum, batchSizeNum, learningRateNum);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
