using System.Diagnostics;

namespace PythonInterpreter
{
	public class ToPython
	{
		/// <summary>
		/// Calls the python function chat.
		/// </summary>
		/// <param name="modelName">Name of the model to be used/loaded</param>
		/// <param name="userInput">Input from the user</param>
		/// <returns>The chatbot's message in response to <paramref name="userInput"/></returns>
		public string ExecuteChatFunction(string modelName, string userInput)
		{
			try
			{
				ProcessStartInfo info = new ProcessStartInfo
				{
					FileName = GetPythonExe(),
					Arguments = string.Format("{0} {1} {2} {3}", ".\\chatbot.py", "chat", modelName, userInput),
					WorkingDirectory = GetSolution(),
					UseShellExecute = false,
					RedirectStandardOutput = true
				};

				using(Process process = Process.Start(info))
				{
					using(StreamReader reader = process.StandardOutput)
					{
						string output = reader.ReadToEnd();
						return output;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return "Encountered error: " + ex;
			}
		}

		/// <summary>
		///	Calls the python function createNewModel.
		/// </summary>
		/// <param name="modelName">Name of the model to be used</param>
		/// <param name="epochsNum">Number of epochs</param>
		/// <param name="batchSizeNum">Batch size number</param>
		/// <param name="learningRateNum">Learning rate number</param>
		/// <param name="hiddenLayers">Name types of the hidden layer, optional</param>
		/// <param name="hiddenLayersValue">Number values used in the hidden layer, optional</param>
		/// <returns>The outcome of the operation</returns>
		public string ExecuteCreateModelFunction(string modelName, int epochsNum, int batchSizeNum, double learningRateNum, string[]? hiddenLayers, int[]? hiddenLayersValue)
		{
			try
			{
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = GetPythonExe(),
                    Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", ".\\chatbot.py", "new_model", modelName, epochsNum, batchSizeNum, learningRateNum, string.Join(" ", (hiddenLayers != null)? hiddenLayers: string.Empty), string.Join(" ", (hiddenLayersValue != null) ? hiddenLayersValue : string.Empty)),
                    WorkingDirectory = GetSolution(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

				using (Process process = Process.Start(info))
				{
					using (StreamReader reader = process.StandardOutput)
					{
						string output = reader.ReadToEnd();
						return output;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
                return "Encountered error: " + ex;
            }
		}

		/// <summary>
		/// Calls the python function train.
		/// </summary>
		/// <param name="modelName">Name of the model</param>
		/// <param name="epochsNum">Number of epochs</param>
		/// <param name="batchSizeNum">Batch size number</param>
		/// <param name="learningRateNum">Learning rate number</param>
		/// <returns>The outcome of the operation</returns>
		public string ExecuteTrainModelFunction(string modelName, int epochsNum, int batchSizeNum, double learningRateNum)
		{
            try
            {
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = GetPythonExe(),
                    Arguments = string.Format("{0} {1} {2} {3} {4} {5}", ".\\chatbot.py", "train", modelName, epochsNum, batchSizeNum, learningRateNum),
                    WorkingDirectory = GetSolution(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };

                using (Process process = Process.Start(info))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string output = reader.ReadToEnd();
                        return output;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return "Encountered error: " + ex;
            }
        }

		/// <summary>
		/// Gets the path to your python executable file using the enviroment variables.
		/// </summary>
		/// <returns>A string that represents the path to your python.exe</returns>
		/// <exception cref="Exception"></exception>
		private string GetPythonExe()
		{
			string[] entries = Environment.GetEnvironmentVariable("path").Split(';');
			string pythonLocation = null;
			foreach (string entry in entries)
			{
				// Must contain python and not scripts
                if (entry.ToLower().Contains("python") && !entry.ToLower().Contains("scripts"))
                {
                    pythonLocation = entry;
					break;
                }
				// Special case, will find the first python scripts path and move up a directory level
				else if(entry.ToLower().Contains("python") && entry.ToLower().Contains("scripts"))
				{
					pythonLocation = Directory.GetParent(entry).FullName;
					break;
				}
            }

			if (string.IsNullOrEmpty(pythonLocation))
			{
				throw new Exception("Unable to find a python executable path, please make sure you have python installed.");
			}
			return pythonLocation + "python.exe";
		}

		/// <summary>
		/// Gets the path to the solution folder.
		/// </summary>
		/// <returns>A string that represents the path to the solution folder</returns>
		/// <exception cref="Exception"></exception>
		private string GetSolution()
		{
			DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
			while (directory != null && !directory.GetFiles("*.sln").Any())
			{
				// Travel up in directory levels
				directory = directory.Parent;
			}

			if (directory == null)
			{
				throw new Exception("Unable to find the .sln file for this project.");
			}
			return directory.FullName;
		}
	}
}
