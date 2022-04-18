using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class CriticalStatistics
    {
        [DataMember]
        public string ItemName;

        [DataMember]
        public int CriticalReadingsCount;
    }
}