using A6IZUK_HSZF_2024251.Application;
using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;
using NUnit.Framework;

namespace A6IZUK_HSZF_2024251.Test
{
    [TestFixture]
    public class RailwayDataTests
    {
        //Ellenőrzi, hogy létezik, és jó-e a fájl
        [Test]
        public void RailwayDataTestLoadingXML1()
        {
            string filePath = "RailwayLines.xml";
            List<RailwayLine> railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath);
            Assert.That(railwayLines.Count(), !Is.EqualTo(0)); 
        }
        //Ellenőrzi, hogy a fájl struktúrája jó-e
        [Test]
        public void RailwayDataTestLoadingXML2()
        {
            string filePath = "RailwayLines.xml";
            List<RailwayLine> railwayLines = XmlDataLoader.LoadRailwayLinesFromXml(filePath);
            string expectedLineNumber = "120A";
            Assert.That(railwayLines[0].LineNumber, Is.EqualTo(expectedLineNumber));
        }
        [Test]
        //Ellenőrzi, hogy létezik, és jó-e a fájl
        public void RailwayDataTestLoadingJSON1()
        {
            string filePath = "RailwayLines.json";
            List<RailwayLine> railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath);
            Assert.That(railwayLines.Count(), !Is.EqualTo(0));
        }
        //Ellenőrzi, hogy a fájl struktúrája jó-e

        [Test]
        public void RailwayDataTestLoadingJSON2()
        {
            string filePath = "RailwayLines.json";
            List<RailwayLine> railwayLines = JsonDataLoader.LoadRailwayLinesFromJson(filePath);
            string expectedLineNumber = "120A";
            Assert.That(railwayLines[0].LineNumber, Is.EqualTo(expectedLineNumber));
        }
    }

    [TestFixture]
    public class RailwayLogicTests
    {

        RailwayLine line = new RailwayLine {LineNumber = "1",LineName ="A"};

        Service service = new Service { TrainNumber = 1, From = "Hagymaföld", To = "Krumpliföld", DelayAmount = 4, TrainType = "Intercity" };

        //Ellenőrzi, hogy jó-e a RailWayService (vasútvonal) hozzáadási funkciója
        [Test]
        public void RailWayServiceRailwayLineAddTest()
        {
            IRailwayService railWayService = new RailwayService();
            railWayService.AddRailwayLines(line);
            Assert.That(railWayService.GetAllRailwayLines(), !Is.EqualTo(0));
        }
        //Ellenőrzi, hogy jó-e a RailWayService (járat) hozzáadási funkciója
        [Test]
        public void RailWayServiceAddTest()
        {
            IRailwayService railWayService = new RailwayService();
            railWayService.AddRailwayLines(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);
            Assert.That(railWayService.GetAllRailwayLines()[0].Services.Count(), !Is.EqualTo(0));
        }
        [Test]
        public void RailWayServiceRailwayModifyTest()
        {
            IRailwayService railWayService = new RailwayService();
            string modifyLinenumber = "2";
            string modifyLinename = "B";
            line = railWayService.ModifyRailway(line, "LINENUMBER", modifyLinenumber);
            line = railWayService.ModifyRailway(line, "LINENAME", modifyLinename);
            Assert.That(line.LineNumber, Is.EqualTo(modifyLinenumber));
            Assert.That(line.LineName, Is.EqualTo(modifyLinename));
        }
        [Test]
        public void RailWayServiceServiceModifyTest()
        {
            IRailwayService railWayService = new RailwayService();

            railWayService.AddRailwayLines(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);

            int serviceIndex = 0;
            string modifyFrom = "Budapest";
            string modifyTo = "Oslo";
            string delayAmount = "10";
            string trainType = "személyi";

            service = railWayService.ModifyService(service, serviceIndex, "FROM", modifyFrom);
            service = railWayService.ModifyService(service, serviceIndex, "TO", modifyTo);
            service = railWayService.ModifyService(service, serviceIndex, "DELAYAMOUNT", delayAmount);
            service = railWayService.ModifyService(service, serviceIndex, "TRAINTYPE", trainType);


            Assert.That(service.To, Is.EqualTo(modifyTo));
            Assert.That(service.From, Is.EqualTo(modifyFrom));
            Assert.That(service.DelayAmount, Is.EqualTo(int.Parse(delayAmount)));
            Assert.That(service.TrainType, Is.EqualTo(trainType));
        }
        [Test]
        public void RailWayServiceDelete()
        {
            IRailwayService railWayService = new RailwayService();

            railWayService.AddRailwayLines(line);

            railWayService.DeleteRailwayLine(line.LineNumber);

            Assert.That(railWayService.GetAllRailwayLines().Count(), Is.EqualTo(0));


        }

        [Test]
        public void RailWayServiceCreateStatistics()
        {
            IRailwayService railWayService = new RailwayService();
            railWayService.AddRailwayLines(line);
            railWayService.AddServiceToRailWayLine(line.LineNumber, service);

            railWayService.CreateStatistics("");

            Assert.That(File.Exists("statistics.txt"), Is.EqualTo(true));
        }
    }

}
