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
        void AddRailwayLines(RailwayLine line);
        void AddServiceToRailWayLine(string lineNumber, Service service);
        List<RailwayLine> GetAllRailwayLines();
        RailwayLine GetRailwayLineByLineNumber(string lineNumber);
        void UpdateRailwayLine(RailwayLine line);
        void DeleteRailwayLine(string lineNumber);
        void CreateStatistics(string outputPath);
    }

}
