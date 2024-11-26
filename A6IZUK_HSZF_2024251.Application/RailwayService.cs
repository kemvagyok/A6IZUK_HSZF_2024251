using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;

namespace A6IZUK_HSZF_2024251.Application
{
    public class RailwayService : IRailwayService
    {

        private readonly IRailWayLineRepository railWayLineRepository;

        public RailwayService(IRailWayLineRepository railWayLineRepository) 
        {
            this.railWayLineRepository = railWayLineRepository;
        }

        public void setAllRailwayLines(List<RailwayLineRaw> lines)
        {
            railWayLineRepository.LoadFromRawData(lines);
        }
        public void AddRailwayLine(RailwayLine line)
        {
            var existingLine = railWayLineRepository.GetByRailwayLineByLineNumber(line.LineNumber);

            if (existingLine == null)
            {
                railWayLineRepository.AddRailwayLine(line);         
            }
            else
            {
                if(existingLine.Services != null)
                {
                    foreach (var service in line.Services)
                    {
                        if (!existingLine.Services.Any(s => s.TrainNumber == service.TrainNumber))
                        {
                            railWayLineRepository.AddRailwayService(service, line.Id);
                        }
                        else
                        {
                            Console.WriteLine("The number of the train exits!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The railway is exits! Nothing is added!");
                }
            }
             
        }

        public void AddServiceToRailWayLine(string lineNumber, Service service)
        {
            var existingLine = railWayLineRepository.GetByRailwayLineByLineNumber(lineNumber);

            if (existingLine == null)
            {
                Console.WriteLine("This railway does not exit!");
            }
            else
            {
                if (existingLine.Services==null || !existingLine.Services.Any(s => s.TrainNumber == service.TrainNumber))
                {
                    railWayLineRepository.AddRailwayService(service, existingLine.Id);
                }
                else
                {
                    Console.WriteLine("The number of the train exits!");
                }

            }
        }

        public List<RailwayLine>  GetAllRailwayLines()
        {
            return railWayLineRepository.GetAll().ToList();
        }
        
        public List<RailwayLine> SearchinRailway(List<CommandSearch> commands)
        {            
            return railWayLineRepository.SearchinRailway(commands);
        }



        public void UpdateRailwayLine(RailwayLine line)
        {
            var existingLine = railWayLineRepository.GetByRailwayLine(line.Id);
            if (existingLine != null)
                railWayLineRepository.UpdateRailwayLine(line);
        }
        public void ModifyRailway(RailwayLine railwayLine, string property, string value)
        {
            if (property.Equals("LINENAME", StringComparison.OrdinalIgnoreCase))
            {
                railwayLine.LineName = value;
                Console.WriteLine($"Railway Line Name modified to: {value}");
            }
            else if (property.Equals("LINENUMBER", StringComparison.OrdinalIgnoreCase))
            {
                railwayLine.LineNumber = value;
                Console.WriteLine($"Railway Line Number modified to: {value}");
            }
            else
            {
                Console.WriteLine($"Invalid railway property: {property}");
            }
            railWayLineRepository.UpdateRailwayLine(railwayLine);
        }

        public void ModifyService(RailwayLine railwayLine, int index, string property, string value)
        {
            Service service = railwayLine.Services.ElementAt(index);
            if (service != null)
            {
                if (property.Equals("FROM", StringComparison.OrdinalIgnoreCase))
                {
                    service.From = value;
                    Console.WriteLine($"Service From modified to: {value}");
                }
                else if (property.Equals("TO", StringComparison.OrdinalIgnoreCase))
                {
                    service.To = value;
                    Console.WriteLine($"Service To modified to: {value}");

                }
                else if (property.Equals("DELAYAMOUNT", StringComparison.OrdinalIgnoreCase) && int.TryParse(value, out int delay))
                {
                    service.DelayAmount = delay;
                    Console.WriteLine($"Service DelayAmount modified to: {delay}");
                }
                else if (property.Equals("TRAINTYPE", StringComparison.OrdinalIgnoreCase))
                {
                    service.TrainType = value;
                    Console.WriteLine($"Service TrainType modified to: {value}");
                }
                else
                {
                    Console.WriteLine($"Invalid service property: {property}");
                }
                railWayLineRepository.UpdateRailwayLine(railwayLine);
            }
            else
            {
                Console.WriteLine("Service is not found! Wrong number!");
            }


            }


            public void DeleteRailwayLine(string lineNumber)
        {
            var line = railWayLineRepository.GetByRailwayLineByLineNumber(lineNumber);
            if (line != null)
                railWayLineRepository.DeleteRailwayLine(line.Id);
            
            
        }

        public void CreateStatistics(string outputPath)
        {
            var statistics = new List<string>();
            foreach (var line in railWayLineRepository.GetAll())
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
                string currentDirectory = Directory.GetCurrentDirectory();
                string newDirectoryPath = Path.Combine(currentDirectory, outputPath);
                if (!Directory.Exists(newDirectoryPath))
                    Directory.CreateDirectory(newDirectoryPath);
                newDirectoryPath += "/statistics.txt";
                System.IO.File.WriteAllLines(newDirectoryPath, statistics);
                Console.WriteLine("Statistics saved successfully to " + outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving statistics: " + ex.Message);
            }

            
        }
    }
}
