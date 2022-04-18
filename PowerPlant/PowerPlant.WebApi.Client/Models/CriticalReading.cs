using System;

namespace PowerPlant.WebApi.Client.Models
{
    public class CriticalReading
    {
        public string LoggedMember { get; set; }
        public string PlantName { get; set; }
        public string ItemName { get; set; }
        public string ParameterName { get; set; }
        public DateTime ReadingTime { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
}