using System.Text.Json;
using System;
using DAL.Models;

namespace chatbot_website.Tools
{

    //Note: keep in mind that the indent.json format has more elements than the Intent object
    public class JsonEditor
    {
        List<Intent> source = new List<Intent>();

        public JsonEditor() { }
        
        public List<Intent> GetJsonListItents(string filepath)
        {
            string json = File.ReadAllText(filepath);
			source = JsonSerializer.Deserialize<List<Intent>>(json);
            return source;
		}

        public void AddToJson(string json_filepath, Intent intent)
        {
            var source = GetJsonListItents(json_filepath);
            source.Add(intent);
            string output = JsonSerializer.Serialize(source, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(json_filepath, output);
		}
    }
}
