using A6IZUK_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace A6IZUK_HSZF_2024251.Persistence.MsSql
{
    public static class XmlDataLoader
    {
        public static List<RailwayLine> LoadRailwayLinesFromXml(string filePath)
        {

            var railwayLines = new List<RailwayLine>();
            try
            {
                XDocument doc = XDocument.Load(filePath);

                foreach (var lineElement in doc.Descendants("RailwayLine"))
                {
                    var railwayLine = new RailwayLine
                    {
                        LineNumber = lineElement.Element("LineNumber")?.Value,
                        LineName = lineElement.Element("LineName")?.Value,
                        Services = new List<Service>()
                    };

                    foreach (var serviceElement in lineElement.Descendants("Service"))
                    {
                        var service = new Service
                        {
                            From = serviceElement.Element("From")?.Value,
                            To = serviceElement.Element("To")?.Value,
                            TrainNumber = int.Parse(serviceElement.Element("TrainNumber")?.Value ?? "0"),
                            DelayAmount = int.Parse(serviceElement.Element("DelayAmount")?.Value ?? "0"),
                            TrainType = serviceElement.Element("TrainType")?.Value
                        };
                        railwayLine.Services.Add(service);
                    }

                    railwayLines.Add(railwayLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while loading XML data: " + ex.Message);
            }

            return railwayLines;
        }
    }
}
