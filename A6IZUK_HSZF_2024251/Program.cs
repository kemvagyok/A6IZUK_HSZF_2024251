using A6IZUK_HSZF_2024251.Application;
using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;



class Program
{
     static void Main(string[] args)
    {
        // Az XML fájl betöltése
        string filePath;        
        List<RailwayLine> railwayLines;

        Console.WriteLine("Üdv a NVSZ programban!\nMelyik formátumból kiván kiolvasni az adatot?\n1. JSON\n2. XML");
        int dataFormute = int.Parse(Console.ReadLine());

        switch(dataFormute)
        {
            case 1:
                filePath = "RailwayLines.json";
                railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath); break;
            case 2:
                filePath = "RailwayLines.xml";
                railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath); break;
            default:
                filePath = "RailwayLines.json";
                railwayLines = new List<RailwayLine>();
                break;
        }

        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var railwayService = serviceProvider.GetService<IRailwayService>();

        // Adatok hozzáadása az alkalmazás indulásakor
        foreach (var line in railwayLines)
        {
            railwayService.AddRailwayLines(line);
        }

        bool running = true;
        while (running)
        {
            Console.WriteLine("1. Add Railway Line");
            Console.WriteLine("2. Add Service");
            Console.WriteLine("3. Modify");
            Console.WriteLine("4. List All Railway Lines");
            Console.WriteLine("5. List by properties");
            Console.WriteLine("6. Generate Statistics");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Enter Line Number: ");
                    var lineNumber = Console.ReadLine();
                    Console.Write("Enter Line Name: ");
                    var lineName = Console.ReadLine();

                    var newLine = new RailwayLine { LineNumber = lineNumber, LineName = lineName };
                    railwayService.AddRailwayLines(newLine);
                    Console.WriteLine("Railway Line Added!");
                    break;


                case "2":
                    Console.WriteLine("Which railway line (number?");
                    string givenLine = Console.ReadLine();
                    Console.WriteLine("[TRAIN NUMBER] [FROM] [TO] [DELAYAMOUNT] [TRAINTYPE]");
                    string[] newServiceSplitted = Console.ReadLine().Split(' ');
                    Service newService = new Service { TrainNumber = int.Parse(newServiceSplitted[0]), From = newServiceSplitted[1], To = newServiceSplitted[2], DelayAmount = int.Parse(newServiceSplitted[3]), TrainType = newServiceSplitted[4] };
                    railwayService.AddServiceToRailWayLine(givenLine, newService);
                    break;

                case "3":

                    Console.WriteLine("Which railway? (number)");
                    string searchedLine = Console.ReadLine();
                    RailwayLine railwayLine = railwayService.GetRailwayLineByLineNumber(searchedLine);
                    if(railwayLine==null)
                    {
                       Console.WriteLine("Invalid choice.");
                        break;
                    }
                    Console.WriteLine($"LINENUMBER: {railwayLine.LineNumber} - LINENAME: {railwayLine.LineName}");
                    int count = 0;
                    foreach (var service in railwayLine.Services)
                    {
                        Console.WriteLine($"{count}   TRAINNUMBER {service.TrainNumber} FROM {service.From} TO {service.To}, DELAY: {service.DelayAmount} minutes");
                        count++;
                    }
                    Console.WriteLine("Railway or one of service (pls, number) do you want to modify?" +
                        "\n RAILWAY [PROPERTY] [VALUE]" +
                        "\n SERVICE [NUMBER SERVICE] [PROPERTY] [VALUE]");

                    string input = Console.ReadLine();
                    var commandParts = input?.Split(' ');

                    if (commandParts != null && commandParts.Length == 3 && commandParts[0].ToUpper() == "RAILWAY")
                    {
                        ModifyRailway(railwayLine, commandParts[1], commandParts[2]);
                    }
                    else if (commandParts != null && commandParts.Length == 4 && (commandParts[0] =="SERVICE" && int.TryParse(commandParts[1], out _)))
                    {
                        int serviceNumber = int.Parse(commandParts[1]);
                        ModifyService(railwayLine, serviceNumber, commandParts[2], commandParts[3]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format.");
                    }
               
                    railwayService.UpdateRailwayLine(railwayLine);
                    break;


                case "4":
                    var lines =  railwayService.GetAllRailwayLines();
                    foreach (var line in lines)
                    {
                        Console.WriteLine($"{line.LineNumber} - {line.LineName}");
                        foreach (var service in line.Services)
                        {
                            Console.WriteLine($"   Train {service.TrainNumber} FROM {service.From} TO {service.To}, DELAY: {service.DelayAmount} minutes");
                        }
                    }
                    break;
                case "5":
                    Console.WriteLine("RAILWAY" +
                        "\n\t[LINEUMBER, LINENAME]\n");
                    Console.WriteLine("SERVICE" +
                        "\n\t[FROM, TO, TRAINNUMBER, DELAYAMOUNT, TRAINTYPE]\n");
                    //TODO: BE KELL FEJEZNED EZT (KERESES)
                    break;


                case "6":
                    Console.Write("Enter output path for statistics file: ");
                    var outputPath = Console.ReadLine();
                    railwayService.CreateStatistics(outputPath);
                    break;

                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        Console.ReadLine();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddScoped<IRailwayService, RailwayService>();
    }

    private static RailwayLine ModifyRailway(RailwayLine railwayLine, string property, string value)
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


    private static RailwayLine ModifyService(RailwayLine railwayLine, int index, string property, string value)
    {

        var service = railwayLine.Services.ElementAt(index);
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
            return railwayLine;
        }

        Console.WriteLine("Service not found with the specified train number.");
        return railwayLine;

    }
}

