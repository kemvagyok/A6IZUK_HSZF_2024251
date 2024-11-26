using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace A6IZUK_HSZF_2024251.Model
{
    public class RailwayLine
    {
        public int Id { get; set; } // Primary key
        public string LineNumber { get; set; }
        public string LineName { get; set; }
 
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
