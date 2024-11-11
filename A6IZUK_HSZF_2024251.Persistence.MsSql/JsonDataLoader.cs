using A6IZUK_HSZF_2024251.Model;
using Newtonsoft.Json;
namespace A6IZUK_HSZF_2024251.Persistence.MsSql
{
    public class JsonDataLoader 
    {
        public static List<RailwayLine> LoadRailwayLinesFromJson(string filePath)
        {
            var railwayLines = new List<RailwayLine>();
            try
            {
                var json = File.ReadAllText(filePath);
                railwayLines = JsonConvert.DeserializeObject<List<RailwayLine>>(json);
                ;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading JSON data: " + ex.Message);
            }

            return railwayLines;
        }
    }
}
