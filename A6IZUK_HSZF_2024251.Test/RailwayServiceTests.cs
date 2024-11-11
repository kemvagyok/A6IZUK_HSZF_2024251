using A6IZUK_HSZF_2024251.Application;
using A6IZUK_HSZF_2024251.Model;
using A6IZUK_HSZF_2024251.Persistence.MsSql;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A6IZUK_HSZF_2024251.Test
{
    public class RailwayServiceTests
    {
      

    //    [Fact]
    //    public  Task AddRailwayLine_ShouldNotifyWhenNewServiceHasLowerDelay()
    //    {
    //        // Arrange
    //        var service = new RailwayService();

    //        var railwayLine = new RailwayLine
    //        {
    //            LineNumber = "100A",
    //            LineName = "BP-Nyugati -> Szolnok",
    //            Services = new List<Service>
    //    {
    //        new Service { TrainNumber = 4320, From = "BP-Nyugati", To = "Cegléd", DelayAmount = 20, TrainType = "Passenger" }
    //    }
    //        };

    //        service.AddRailwayLines(railwayLine);

    //        // Act - új vonat hozzáadása, amely kevesebbet késik
    //        var newLine = new RailwayLine
    //        {
    //            LineNumber = "100A",
    //            LineName = "BP-Nyugati -> Szolnok",
    //            Services = new List<Service>
    //    {
    //        new Service { TrainNumber = 4330, From = "BP-Nyugati", To = "Cegléd", DelayAmount = 5, TrainType = "Passenger" }
    //    }
    //        };

    //        using (var sw = new StringWriter())
    //        {
    //            Console.SetOut(sw);

    //             service.AddRailwayLines(newLine);

    //            var output = sw.ToString();
    //            Assert.Contains("kevesebbet késett", output);
    //        }
    //    }
    //    [Fact]
    //    public  Task GetAllRailwayLines_ShouldReturnAllLines()
    //    {
    //        // Arrange
            
    //        var service = new RailwayService();
    //        context.RailwayLines.Add(new RailwayLine { LineNumber = "100A", LineName = "BP-Nyugati -> Szolnok" });
    //        context.RailwayLines.Add(new RailwayLine { LineNumber = "120A", LineName = "BP-Keleti -> Szolnok" });
    //         context.SaveChanges();

    //        // Act
    //        var result =  service.GetAllRailwayLines();

    //        // Assert
    //        Assert.Equal(2, result.Count);
    //    }

    //    [Fact]
    //    public  Task GetRailwayLineById_ShouldReturnCorrectLine()
    //    {
    //        // Arrange
            
    //        var service = new RailwayService();
    //        var railwayLine = new RailwayLine { LineNumber = "100A", LineName = "BP-Nyugati -> Szolnok" };
    //        context.RailwayLines.Add(railwayLine);
          
    //        // Act
    //        var result =  service.GetRailwayLineById(railwayLine.LineNumber);

    //        // Assert
    //        Assert.NotNull(result);
    //        Assert.Equal("100A", result.LineNumber);
    //    }

    //    [Fact]
    //    public  Task UpdateRailwayLine_ShouldUpdateLineSuccessfully()
    //    {
    //        // Arrange
            
    //        var service = new RailwayService();
    //        var railwayLine = new RailwayLine { LineNumber = "100A", LineName = "BP-Nyugati -> Szolnok" };
    //        context.RailwayLines.Add(railwayLine);
    //         context.SaveChanges();

    //        // Act
    //        railwayLine.LineName = "BP-Nyugati -> Szolnok Updated";
    //         service.UpdateRailwayLine(railwayLine);
    //        var result =  context.RailwayLines.FirstOrDefault(r => r.LineNumber == railwayLine.LineNumber);

    //        // Assert
    //        Assert.NotNull(result);
    //        Assert.Equal("BP-Nyugati -> Szolnok Updated", result.LineName);
    //    }

    //    [Fact]
    //    public  Task DeleteRailwayLine_ShouldDeleteLineSuccessfully()
    //    {
    //        // Arrange
            
    //        var service = new RailwayService();
    //        var railwayLine = new RailwayLine { LineNumber = "200A", LineName = "TestLine" };

    //        context.RailwayLines.Add(railwayLine);
    //        context.SaveChanges();

    //        // Act
    //        service.DeleteRailwayLine(railwayLine.LineNumber);
    //        var result =  context.RailwayLines.FirstOrDefault(r => r.LineNumber == railwayLine.LineNumber);

    //        // Assert
    //        Assert.Null(result);
    //    }
    }
      
}
