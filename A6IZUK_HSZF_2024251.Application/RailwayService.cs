using A6IZUK_HSZF_2024251.Model;
using System.Collections.Generic;

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
        
        public List<RailwayLine> SearchinRailway(List<CommandSearch> commands)
        {
            List<RailwayLine> railwayLines = new List<RailwayLine>();

            List<string> commandsofRailway = new List<string>() { "LINENUMBER", "LINENAME" };
            List<string> commandsofService = new List<string>() { "FROM", "TO", "TRAINNUMBER", "DELAYAMOUNT", "TRAINTYPE" };
            foreach (var command in commands)
            {
                if(command.Type == "RAILWAY")
                {
                    foreach (var commandProperty in commandsofRailway)
                    if (command.Property== commandProperty)
                    {
                            var selectedList = _railwayLines
                                .Where(x =>
                                {
                                    if (command.Property == "LINENAME") return x.LineName == command.Value;
                                    if (command.Property == "LINENUMBER") return x.LineNumber.ToString() == command.Value;
                                    return false;
                                })
                                .ToList();
                            if(selectedList is not null)
                                railwayLines.AddRange(selectedList);
                        }
                   
                }
                if (command.Type == "SERVICE")
                {
                    foreach (var commandProperty in commandsofService)
                        if (command.Property == commandProperty)
                        {
                            foreach(var railwayLine in _railwayLines)
                            {
                                var selectedList = railwayLine.Services
                                .Where(x =>
                                {
                                    if (command.Property == "FROM") return x.From == command.Value;
                                    if (command.Property == "TO") return x.To == command.Value;
                                    if (command.Property == "TRAINNUMBER") return x.TrainNumber.ToString() == command.Value;
                                    if (command.Property == "DELAYAMOUNT") return x.DelayAmount <= int.Parse(command.Value);
                                    if (command.Property == "TRAINTYPE") return x.TrainType == command.Value;

                                    return false;
                                })
                                .ToList();

                                if (selectedList.Count != 0)
                                {
                                    railwayLine.Services = selectedList ;           
                                    railwayLines.Add(railwayLine);
                                }

                            }
                        }
                }
            }

            return railwayLines;
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
        public RailwayLine ModifyRailway(RailwayLine railwayLine, string property, string value)
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
            return railwayLine;
        }

        public Service ModifyService(Service service, int index, string property, string value)
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
                return service;
            

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
