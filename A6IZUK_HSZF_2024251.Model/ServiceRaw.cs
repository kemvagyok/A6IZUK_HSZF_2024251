using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A6IZUK_HSZF_2024251.Model
{
    public class ServiceRaw
    {
        public string From { get; set; }
        public string To { get; set; }
        public int TrainNumber { get; set; }
        public int DelayAmount { get; set; }
        public string TrainType { get; set; }
    }
}
