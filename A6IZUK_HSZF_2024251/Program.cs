using A6IZUK_HSZF_2024251.Application;
using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



class Program
{
     static void Main(string[] args)
    {
        // Az XML, vagy JSON fájl betöltése
        string filePath;        
        List<RailwayLineRaw> railwayLines;

        Console.WriteLine("Welcome to the NVSZ programme!\nWhich format do you want to import the data from?\n1. JSON\n2. XML");
        int dataFormatChoosen = int.Parse(Console.ReadLine());

        switch(dataFormatChoosen)
        {
            case 1:
                filePath = "RailwayLines.json";
                railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath); break;
            case 2:
                filePath = "RailwayLines.xml";
                railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath); break;
            default:
                filePath = "RailwayLines.json";
                railwayLines = new List<RailwayLineRaw>();
                break;
        }

        var services = new ServiceCollection();

        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        var railwayService = serviceProvider.GetService<IRailwayService>();
        railwayService.setAllRailwayLines(railwayLines);

        // Adatok hozzáadása az alkalmazás indulásakor

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
                    railwayService.AddRailwayLine(newLine);
                    Console.WriteLine("Railway Line Added!");
                    break;


                case "2":
                    
                    Console.WriteLine("Which railway line (number?");
                    string givenLine = Console.ReadLine();
                    Console.WriteLine("[TRAIN NUMBER] [FROM] [TO] [DELAYAMOUNT] [TRAINTYPE]");
                    string[] newServiceSplitted = Console.ReadLine().Split(' ');
                    Service newService = new Service { TrainNumber = int.Parse(newServiceSplitted[0]), From = newServiceSplitted[1], To = newServiceSplitted[2], DelayAmount = int.Parse(newServiceSplitted[3]), TrainType = newServiceSplitted[4]};
                    railwayService.AddServiceToRailWayLine(givenLine, newService);
                    break;

                case "3":

                    Console.WriteLine("Which railway? (number)");
                    string searchedLine = Console.ReadLine();
                    RailwayLine railwayLine = railwayService.GetAllRailwayLines().FirstOrDefault(r => r.LineNumber == searchedLine);
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
                       railwayService.ModifyRailway(railwayLine, commandParts[1], commandParts[2]);
                    }
                    else if (commandParts != null && commandParts.Length == 4 && (commandParts[0] =="SERVICE" && int.TryParse(commandParts[1], out _)))
                    {
                        int serviceIndex = int.Parse(commandParts[1]);
                         railwayService.ModifyService(railwayLine, serviceIndex, commandParts[2], commandParts[3]);
                    }
                    else
                    {
                        Console.WriteLine("Invalid input format.");
                    }
              
                    break;


                case "4":
                    var lines =  railwayService.GetAllRailwayLines();
                    foreach (var line in lines)
                    {
                        Console.WriteLine($"{line.LineNumber} - {line.LineName}");
                        if (line.Services == null)
                        {
                            Console.WriteLine($"  There is not services!");
                        }
                        else
                        {
                            foreach (var service in line.Services)
                            {
                                Console.WriteLine($"   Train {service.TrainNumber} FROM {service.From} TO {service.To}, DELAY: {service.DelayAmount} minutes");
                            }
                        }
                    }
                    break;
                case "5":
                    List<CommandSearch> commandSearches = new List<CommandSearch>();

                    bool seachRunning = true;
                    while (seachRunning)
                    {
                        Console.WriteLine("TYPE PROPERTY VALUE\n");
                        Console.WriteLine("RAILWAY" +
                        "\n\t[LINENUMBER, LINENAME]\n");
                        Console.WriteLine("SERVICE" +
                            "\n\t[FROM, TO, TRAINNUMBER, DELAYAMOUNT, TRAINTYPE]\n");
                        Console.WriteLine("IF YOU WANT EXIT FROM THIS COMMAND OR FINISH THE WRITING AND SEE THE RESULT:" +
                            "\n\tEXIT\n");
                        string[] splittedCommands = Console.ReadLine().Split(' ');
                        if (splittedCommands[0] == "EXIT")
                        {
                            seachRunning = false;
                            if(commandSearches.Count > 0)
                            {
                                var selectedList = railwayService.SearchinRailway(commandSearches);
                                foreach (var line in selectedList)
                                {
                                    Console.WriteLine($"{line.LineNumber} - {line.LineName}");
                                    foreach (var service in line.Services)
                                    {
                                        Console.WriteLine($"   Train {service.TrainNumber} FROM {service.From} TO {service.To}, DELAY: {service.DelayAmount} minutes");
                                    }
                                }
                            }

                        }
                        else
                        {
                            CommandSearch search = new CommandSearch { Type = splittedCommands[0], Property = splittedCommands[1], Value = splittedCommands[2] };
                            commandSearches.Add(search);
                        }
                    }
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
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=False;Initial Catalog=NVSZdb;MultipleActiveResultSets=true;Trusted_Connection=True;";
        services.AddDbContext<RailWayLineDbContext>(options =>
    options.UseSqlServer(connStr));
        services.AddScoped<IRailWayLineRepository, RailWayLineRepository>();
        services.AddScoped<IRailwayService, RailwayService>();


    }

}

