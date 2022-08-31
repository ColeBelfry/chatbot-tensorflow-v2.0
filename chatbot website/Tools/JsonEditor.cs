using System.Text.Json;
using System;
using DAL.Models;
using System.Security.Cryptography.X509Certificates;

namespace chatbot_website.Tools
{

    //Note: keep in mind that the indent.json format has more elements than the Intent object
    public class JsonEditor
    {
        Intents intentsList = new Intents(); 
       

        public JsonEditor() { }
        
        public List<Intent> GetJsonListItents(string filepath)
        {
            string json = File.ReadAllText(filepath);
            
			intentsList = JsonSerializer.Deserialize<Intents>(json);
			List<Intent> source = new List<Intent>();
			foreach (Intent intent in intentsList.intents)
            {
                source.Add(intent);
            }
            return source;
		}

        public void AddToJson(string json_filepath, Intent intent)
        {
            //var source = GetJsonListItents(json_filepath);
            //source.Add(intent);
            intentsList.intents.Add(intent);
            string output = JsonSerializer.Serialize(intentsList, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(json_filepath, output);
		}
    }
}
