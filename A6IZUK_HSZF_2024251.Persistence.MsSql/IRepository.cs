using A6IZUK_HSZF_2024251.Model;

namespace A6IZUK_HSZF_2024251.Persistence.MsSql
{
    public interface IRailWayLineRepository
    {
        IEnumerable<RailwayLine> GetAll();
        RailwayLine GetByRailwayLine(int id);
        RailwayLine GetByRailwayLineByLineNumber(string lineNumber);
        void AddRailwayLine(RailwayLine railwayLine);
        void AddRailwayService(Service service, int id);

        void UpdateRailwayLine(RailwayLine railwayLine);
        void DeleteRailwayLine(int id);

        List<RailwayLine> SearchinRailway(List<CommandSearch> commands);
        void LoadFromRawData(List<RailwayLineRaw> railwayLines);
    }

}
