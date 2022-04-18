using System;
using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class CriticalReading
    {
        [DataMember]
        public string LoggedMember;

        [DataMember]
        public string PlantName;

        [DataMember]
        public string ItemName;

        [DataMember]
        public string ParameterName;

        [DataMember]
        public DateTime ReadingTime;

        [DataMember]
        public double MinValue;

        [DataMember]
        public double MaxValue;
    }
}