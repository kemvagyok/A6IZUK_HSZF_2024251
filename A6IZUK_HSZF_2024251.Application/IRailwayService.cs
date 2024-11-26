using A6IZUK_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A6IZUK_HSZF_2024251.Application
{
    public interface IRailwayService
    {
        public void setAllRailwayLines(List<RailwayLineRaw> lines);
        void AddRailwayLine(RailwayLine line);
        void AddServiceToRailWayLine(string lineNumber, Service service);
        List<RailwayLine> GetAllRailwayLines();
        void UpdateRailwayLine(RailwayLine line);
        void DeleteRailwayLine(string lineNumber);
        void ModifyRailway(RailwayLine railwayLine, string property, string value);
        void ModifyService(RailwayLine railwayLine, int index, string property, string value);
        public List<RailwayLine> SearchinRailway(List<CommandSearch> commands);
        void CreateStatistics(string outputPath);
    }

}
