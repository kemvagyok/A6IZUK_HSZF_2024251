using A6IZUK_HSZF_2024251.Application;
using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace A6IZUK_HSZF_2024251.Test
{
    [TestFixture]
    public class RailwayDataTests
    {
        private RailWayLineDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RailWayLineDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRailwayDb")
                .Options;
            return new RailWayLineDbContext(options);
        }

        //Ellenőrzi, hogy létezik, és jó-e a fájl
        [Test]
        public void RailwayDataTestLoadingXML1()
        {
            string filePath = "RailwayLines.xml";
            List<RailwayLineRaw> railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath);
            Assert.That(railwayLines.Count(), !Is.EqualTo(0)); 
        }
        //Ellenőrzi, hogy a fájl struktúrája jó-e
        [Test]
        public void RailwayDataTestLoadingXML2()
        {
            string filePath = "RailwayLines.xml";
            List<RailwayLineRaw> railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath);
            string expectedLineNumber = "120A";
            Assert.That(railwayLines[0].LineNumber, Is.EqualTo(expectedLineNumber));
        }
        [Test]
        //Ellenőrzi, hogy létezik, és jó-e a fájl
        public void RailwayDataTestLoadingJSON1()
        {
            string filePath = "RailwayLines.json";
            List<RailwayLineRaw> railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath);
            Assert.That(railwayLines.Count(), !Is.EqualTo(0));
        }
        //Ellenőrzi, hogy a fájl struktúrája jó-e

        [Test]
        public void RailwayDataTestLoadingJSON2()
        {
            string filePath = "RailwayLines.json";
            List<RailwayLineRaw> railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath);
            string expectedLineNumber = "120A";
            Assert.That(railwayLines[0].LineNumber, Is.EqualTo(expectedLineNumber));
        }
        [Test]
        public void RailwayDataTestLoadingDB()
        {
            string filePath = "RailwayLines.json";
            List<RailwayLineRaw> railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath);
            string expectedLineNumber = "120A";
            Assert.That(railwayLines[0].LineNumber, Is.EqualTo(expectedLineNumber));
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);
            railWayService.setAllRailwayLines(railwayLines);
            Assert.That(railWayService.GetAllRailwayLines()[0].LineNumber, Is.EqualTo(expectedLineNumber));    
        }
    }

    [TestFixture]
    public class RailwayLogicTests
    {

        private RailWayLineDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RailWayLineDbContext>()
                .UseInMemoryDatabase(databaseName: "TestRailwayDb")
                .Options;
            return new RailWayLineDbContext(options);
        }

        //Ellenőrzi, hogy jó-e a RailWayService (vasútvonal) hozzáadási funkciója
        [Test]
        public void RailWayServiceRailwayLineAddTest()
        {
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };
            railWayService.AddRailwayLine(line);
            Assert.That(railWayService.GetAllRailwayLines(), !Is.EqualTo(0));
        }
        //Ellenőrzi, hogy jó-e a RailWayService (járat) hozzáadási funkciója
        [Test]
        public void RailWayServiceAddTest()
        {
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };
            railWayService.AddRailwayLine(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);
            Assert.That(railWayService.GetAllRailwayLines()[0].Services.Count(), !Is.EqualTo(0));
        }
        //Ellenőrzi, hogy az adott vonal adott jellemzoje modosult-e
        [Test]
        public void RailWayServiceRailwayModifyTest()
        {
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };
            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };
            string modifyLinenumber = "2";
            string modifyLinename = "B";
            railWayService.AddRailwayLine(line);
            railWayService.ModifyRailway(line, "LINENUMBER", modifyLinenumber);
            railWayService.ModifyRailway(line, "LINENAME", modifyLinename);
            Assert.That(line.LineNumber, Is.EqualTo(modifyLinenumber));
            Assert.That(line.LineName, Is.EqualTo(modifyLinename));
            railWayService.AddRailwayLine(line);
            railWayService.UpdateRailwayLine(line);
            Assert.That(railWayService.GetAllRailwayLines()[0].LineName, Is.EqualTo(modifyLinename));
            Assert.That(railWayService.GetAllRailwayLines()[0].LineNumber, Is.EqualTo(modifyLinenumber));
        }
        //Ellenőrzi, hogy az adott járat adott jellmezője modosult-e
        [Test]
        public void RailWayServiceServiceModifyTest()
        {

            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };

            railWayService.AddRailwayLine(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);

            int serviceIndex = 0;
            string modifyFrom = "Budapest";
            string modifyTo = "Oslo";
            string delayAmount = "10";
            string trainType = "személyi";

            railWayService.ModifyService(line, serviceIndex, "FROM", modifyFrom);
            railWayService.ModifyService(line, serviceIndex, "TO", modifyTo);
            railWayService.ModifyService(line, serviceIndex, "DELAYAMOUNT", delayAmount);
            railWayService.ModifyService(line, serviceIndex, "TRAINTYPE", trainType);


            Assert.That(service.To, Is.EqualTo(modifyTo));
            Assert.That(service.From, Is.EqualTo(modifyFrom));
            Assert.That(service.DelayAmount, Is.EqualTo(int.Parse(delayAmount)));
            Assert.That(service.TrainType, Is.EqualTo(trainType));

        }
        //Ellenőrzi, hogy az adott járatot kitörli-e
        [Test]
        public void RailWayServiceDelete()
        {

            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };

            railWayService.AddRailwayLine(line);

            railWayService.DeleteRailwayLine(line.LineNumber);

            Assert.That(railWayService.GetAllRailwayLines().Count(), Is.EqualTo(0));


        }
        //Ellenőrzi, hogy egyáltalán kiirja a fájlt.
        [Test]
        public void RailWayServiceCreateStatistics()
        {
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };
            railWayService.AddRailwayLine(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);

            railWayService.CreateStatistics("");

            Assert.That(File.Exists("statistics.txt"), Is.EqualTo(true));
        }


        [Test]
        public void SearchinRailwayTest()
        {
            var context = GetInMemoryDbContext();
            var repository = new RailWayLineRepository(context);
            IRailwayService railWayService = new RailwayService(repository);

            RailwayLine line = new RailwayLine { LineNumber = "1", LineName = "A" };

            Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };
            railWayService.AddRailwayLine(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);

            RailwayLine line1 = new RailwayLine { LineNumber = "2", LineName = "B" };
            RailwayLine line2 = new RailwayLine { LineNumber = "3", LineName = "C" };

            Service service1_1 = new Service { TrainNumber = 3, From = "Asd", To = "Dsa", DelayAmount = 4, TrainType = "Intercity",RailwayLine = line1, RailwayLineId = line1.Id };
            Service service1_2 = new Service { TrainNumber = 4, From = "Asd", To = "Pokol", DelayAmount = 4, TrainType = "Intercity", RailwayLine = line1, RailwayLineId = line1.Id };
            Service service1_3 = new Service { TrainNumber = 5, From = "Asd", To = "Mennyország", DelayAmount = 4, TrainType = "Zónázó", RailwayLine = line1, RailwayLineId = line1.Id };
            Service service1_4 = new Service { TrainNumber = 6, From = "Kaposvár", To = "Szeged", DelayAmount = 10, TrainType = "Személyi", RailwayLine = line1, RailwayLineId = line1.Id };
            line1.Services.Add(service1_1);
            line1.Services.Add(service1_2);
            line1.Services.Add(service1_3);
            line1.Services.Add(service1_4);

            railWayService.AddServiceToRailWayLine(service1_1.RailwayLine.LineNumber,service1_1);
            railWayService.AddServiceToRailWayLine(service1_2.RailwayLine.LineNumber,service1_2);
            railWayService.AddServiceToRailWayLine(service1_3.RailwayLine.LineNumber,service1_3);
            railWayService.AddServiceToRailWayLine(service1_4.RailwayLine.LineNumber,service1_4);

            railWayService.AddRailwayLine(line1);
            railWayService.AddRailwayLine(line2);

            //Megnézzük, hogy létezik-e az a járat, aminek a száma 3, és keressük összes olyan járatot, amiben a cél Asd (függetlenül, hogy milyen vonal)
            CommandSearch search1 = new CommandSearch { Type = "RAILWAY", Property = "LINENUMBER", Value = "3" };
            CommandSearch search2 = new CommandSearch { Type = "SERVICE", Property = "FROM", Value = "Asd" };
            
            List<CommandSearch> commandSearches = new List<CommandSearch> { search1, search2 };
            
            List<RailwayLine> selectedRailways = railWayService.SearchinRailway(commandSearches);
            
            var selected1Railway = selectedRailways?.First(x=>x.LineNumber=="2");
            Assert.That(selected1Railway, !Is.EqualTo(null));
            Assert.That(selected1Railway.Services.Count, Is.EqualTo(3));

            //Megnézzük, hogy vannak-e 5-nél kevesebbiek
            CommandSearch search3 = new CommandSearch { Type = "SERVICE", Property = "DELAYAMOUNT", Value = "5" };

            commandSearches = new List<CommandSearch> { search3 };

            selectedRailways = railWayService.SearchinRailway(commandSearches);
            //Ellenőrzi, hogy nem üres-e
            Assert.That(selectedRailways, !Is.EqualTo(null));
            int count = 0;
            foreach (RailwayLine actualLine in selectedRailways)
                count+= actualLine.Services.Count;
            //Azért 4, mert 5-nél kevesebbi késési járatok száma 4.
            Assert.That(count, Is.EqualTo(4));


        }
    }

}
