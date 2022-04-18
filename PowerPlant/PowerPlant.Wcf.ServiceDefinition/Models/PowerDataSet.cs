using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class PowerDataSet
    {
        [DataMember]
        public string Name;

        [DataMember]
        public double CurrentValue;

        [DataMember]
        public double EnergyProduced;
    }
}