using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace A6IZUK_HSZF_2024251.Model
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key

        public string From { get; set; }
        public string To { get; set; }
        public int TrainNumber { get; set; }
        public int DelayAmount { get; set; }
        public string TrainType { get; set; }
        public virtual RailwayLine RailwayLine { get; set; }
        public virtual int RailwayLineId { get; set; }
    }
}
