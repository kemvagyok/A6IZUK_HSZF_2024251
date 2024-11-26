using A6IZUK_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;


namespace A6IZUK_HSZF_2024251.Persistence.MsSql
{
    public class RailWayLineRepository : IRailWayLineRepository
    {
        RailWayLineDbContext ctx;

        public RailWayLineRepository(RailWayLineDbContext ctx)
        {
            this.ctx = ctx;
        }
        public void AddRailwayLine(RailwayLine railwayLine)
        {
            ctx.RailwaysLines.Add(railwayLine);
            ctx.SaveChanges();

        }

        public void AddRailwayService(Service service, int id)
        {
       
            RailwayLine railwayLine = ctx.RailwaysLines.First(x => x.Id == id);
            service.RailwayLineId = id;
            service.RailwayLine = railwayLine;
            railwayLine?.Services.Add(service);
            ctx.SaveChanges();
        }

        public void DeleteRailwayLine(int id)
        {
            var railWayLineToDelete = ctx.RailwaysLines.FirstOrDefault(t => t.Id == id);
            ctx.RailwaysLines.Remove(railWayLineToDelete);
            ctx.SaveChanges();
        }

        public IEnumerable<RailwayLine> GetAll()
        {
            return ctx.RailwaysLines;
        }
        public RailwayLine GetByRailwayLineByLineNumber(string lineNumber)
        {
            return ctx.RailwaysLines.FirstOrDefault(x => x.LineNumber == lineNumber);
        }

        public RailwayLine GetByRailwayLine(int id)
        {
            return ctx.RailwaysLines.First(x=>x.Id == id);
        }

        public void UpdateRailwayLine(RailwayLine railwayLineP)
        {
            RailwayLine railwayLine = ctx.RailwaysLines.First(x => x.Id == railwayLineP.Id);
            railwayLine.LineName = railwayLineP.LineName;
            railwayLine.Services = railwayLineP.Services;
            ctx.SaveChanges();

        }

        public List<RailwayLine> SearchinRailway(List<CommandSearch> commands)
        {
            IQueryable<RailwayLine> query = ctx.RailwaysLines.Include(rl => rl.Services);

            List<RailwayLine> result = new List<RailwayLine>();

            foreach (var command in commands)
            {
              
                if (command.Type == "RAILWAY")
                {
                    if (command.Property == "LINENAME")
                    {
                        result.AddRange(query.Where(rl => rl.LineName == command.Value));
                    }
                    else if (command.Property == "LINENUMBER")
                    {
                        result.AddRange(query.Where(rl => rl.LineNumber.ToString() == command.Value));
                    }
                }
                else if (command.Type == "SERVICE")
                {
                    if (command.Property == "FROM")
                    {
                        foreach(var rl in query)
                        {
                            var local_rl = new RailwayLine() {Id=rl.Id, LineNumber = rl.LineNumber, LineName = rl.LineName};
                            local_rl.Services = rl.Services.Where(x => x.From == command.Value).ToList(); 
                            if(local_rl.Services.Count > 0)
                                result.Add(local_rl);              
                        }
                    }
                    else if (command.Property == "TO")
                    {
                        foreach (var rl in query)
                        {
                            var local_rl = new RailwayLine() { Id = rl.Id, LineNumber = rl.LineNumber, LineName = rl.LineName };
                            local_rl.Services = rl.Services.Where(x => x.To == command.Value).ToList();
                            if (local_rl.Services.Count > 0)
                                result.Add(local_rl);
                        }
                    }
                    else if (command.Property == "TRAINNUMBER")
                    {
                        foreach (var rl in query)
                        {
                            var local_rl = new RailwayLine() { Id = rl.Id, LineNumber = rl.LineNumber, LineName = rl.LineName };
                            local_rl.Services = rl.Services.Where(x => x.TrainNumber == int.Parse(command.Value)).ToList();
                            if (local_rl.Services.Count > 0)
                                result.Add(local_rl);
                        }
                    }
                    else if (command.Property == "DELAYAMOUNT")
                    {
                        if (int.TryParse(command.Value, out int delay))
                        {
                            foreach (var rl in query)
                            {
                                var local_rl = new RailwayLine() { Id = rl.Id, LineNumber = rl.LineNumber, LineName = rl.LineName };
                                local_rl.Services = rl.Services.Where(x => x.DelayAmount < int.Parse(command.Value)).ToList();
                                if (local_rl.Services.Count > 0)
                                    result.Add(local_rl);
                            }
                        }
                    }
                    else if (command.Property == "TRAINTYPE")
                    {
                        foreach (var rl in query)
                        {
                            var local_rl = new RailwayLine() { Id = rl.Id, LineNumber = rl.LineNumber, LineName = rl.LineName };
                            local_rl.Services = rl.Services.Where(x => x.TrainType == command.Value).ToList();
                            if (local_rl.Services.Count > 0)
                                result.Add(local_rl);
                        }
                    }
                }
            }

            // Az eredmény lekérdezése
            return result;
        }

        public void LoadFromRawData(List<RailwayLineRaw> railwayLines)
        {
            foreach (var railwayLine in railwayLines)
            {
                RailwayLine line = new RailwayLine() {LineName = railwayLine.LineName,LineNumber = railwayLine.LineNumber};
                ctx.RailwaysLines.Add(line);
                ctx.SaveChanges();
                line = ctx.RailwaysLines.FirstOrDefault(x => x.LineNumber == line.LineNumber);
                var services = new List<Service>();
                foreach (var service in railwayLine.Services)
                {
                    var newService = new Service() { From = service.From, To = service.To, TrainNumber = service.TrainNumber, DelayAmount = service.DelayAmount, TrainType = service.TrainType, RailwayLine = line, RailwayLineId = line.Id };
                    services.Add(newService);
                    ctx.Services.Add(newService);
                    ctx.SaveChanges();
                }
                line.Services = services;
                ctx.SaveChanges();

            }


        }
    }
}
