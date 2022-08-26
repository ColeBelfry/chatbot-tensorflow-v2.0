using System.Diagnostics;

namespace PythonInterpreter
{
	public class ToPython
	{
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

		public string ExecuteCreateModelFunction(string modelName, int epochsNum, int batchSizeNum, int learningRateNum, string[] hiddenLayers, int[] hiddenLayersValue)
		{
			try
			{
                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = GetPythonExe(),
                    Arguments = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", ".\\chatbot.py", "new_model", modelName, epochsNum, batchSizeNum, learningRateNum, string.Join(" ", hiddenLayers), string.Join(" ", hiddenLayersValue)),
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

		public string ExecuteTrainModelFunction(string modelName, int epochsNum, int batchSizeNum, int learningRateNum)
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

		private string GetPythonExe()
		{
			string[] entries = Environment.GetEnvironmentVariable("path").Split(';');
			string pythonLocation = null;
			foreach (string entry in entries)
			{
                if (entry.ToLower().Contains("python") && !entry.ToLower().Contains("scripts"))
                {
                    pythonLocation = entry;
					break;
                }
            }

			if (string.IsNullOrEmpty(pythonLocation))
			{
				throw new Exception("Unable to find a python executable path, please make sure you have python installed.");
			}
			return pythonLocation + "python.exe";
		}

		private string GetSolution()
		{
			DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
			while (directory != null && !directory.GetFiles("*.sln").Any())
			{
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
