using A6IZUK_HSZF_2024251.Model;

namespace A6IZUK_HSZF_2024251.Application
{
    public class RailwayService : IRailwayService
    {

        private readonly List<RailwayLine> _railwayLines = new List<RailwayLine>();


        public void AddRailwayLines(RailwayLine line)
        {
            var existingLine = _railwayLines.FirstOrDefault(r => r.LineNumber == line.LineNumber);

            if (existingLine == null)
            {
                _railwayLines.Add(line);
            }
            else
            {
                foreach (var service in line.Services)
                {
                    if (!existingLine.Services.Any(s => s.TrainNumber == service.TrainNumber))
                    {
                        existingLine.Services.Add(service);
                    }
                    else
                    {
                        Console.WriteLine("The number of the train exits!");
                    }
                }
            }
             
        }

        public void AddServiceToRailWayLine(string lineNumber, Service service)
        {
            var existingLine = _railwayLines.FirstOrDefault(r => r.LineNumber == lineNumber);

            if (existingLine == null)
            {
                Console.WriteLine("This railway does not exit!");
            }
            else
            {
                if (!existingLine.Services.Any(s => s.TrainNumber == service.TrainNumber))
                {
                    existingLine.Services.Add(service);
                }
                else
                {
                    Console.WriteLine("The number of the train exits!");
                }

            }
        }

        public List<RailwayLine>  GetAllRailwayLines()
        {
            return _railwayLines;
        }
        //TODO: KERESÉSÉ
        public static List<RailwayLine> SearchinRailway(string[] properties)
        {
            List<RailwayLine> railwayLines = new List<RailwayLine>();

            //TODO: BE KELL FEJEZNED EZT (KERESES)

            return railwayLines;
        }

        public RailwayLine  GetRailwayLineByLineNumber(string lineNumber)
        {
            return _railwayLines.FirstOrDefault(r => r.LineNumber == lineNumber);
        }



        public void UpdateRailwayLine(RailwayLine line)
        {
            var existingLine = _railwayLines.FirstOrDefault(r => r.LineNumber == line.LineNumber);
            if (existingLine != null)
            {
                existingLine.LineNumber = line.LineNumber;
                existingLine.LineName = line.LineName;
                existingLine.Services = line.Services;
            }
            
        }

        public void DeleteRailwayLine(string lineNumber)
        {
            var line = _railwayLines.FirstOrDefault(r => r.LineNumber == lineNumber);
            if (line != null)
            {
                _railwayLines.Remove(line);
            }
            
        }

        public void CreateStatistics(string outputPath)
        {
            var statistics = new List<string>();
            foreach (var line in _railwayLines)
            {
                var onTimeCount = line.Services.Count(service => service.DelayAmount < 5);
                var averageDelay = line.Services.Average(service => service.DelayAmount);
                var mostDelayedService = line.Services.OrderByDescending(service => service.DelayAmount).FirstOrDefault();
                var mostFrequentDestination = line.Services
                    .Where(service => service.DelayAmount > 5)
                    .GroupBy(service => service.To)
                    .OrderByDescending(group => group.Count())
                    .FirstOrDefault()?.Key;

                statistics.Add($"Line {line.LineNumber} ({line.LineName}):");
                statistics.Add($" - On-time (<5 min delay) services: {onTimeCount}");
                statistics.Add($" - Average delay: {averageDelay:F2} minutes");
                if (mostDelayedService != null)
                {
                    statistics.Add($" - Most delayed service: Train {mostDelayedService.TrainNumber} with {mostDelayedService.DelayAmount} minutes delay");
                }
                if (mostFrequentDestination != null)
                {
                    statistics.Add($" - Most frequent delayed destination: {mostFrequentDestination}");
                }
                statistics.Add("");
            }

            try
            {
                outputPath += "statistics.txt";
                System.IO.File.WriteAllLines(outputPath, statistics);
                Console.WriteLine("Statistics saved successfully to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving statistics: " + ex.Message);
            }

            
        }
    }
}
