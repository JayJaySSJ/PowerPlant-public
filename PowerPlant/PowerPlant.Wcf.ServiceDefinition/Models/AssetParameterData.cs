using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class AssetParameterData
    {
        [DataMember]
        public double MinValue;

        [DataMember]
        public double MaxValue;

        [DataMember]
        public double TypicalValue;

        [DataMember]
        public double CurrentValue;

        [DataMember]
        public string Unit;
    }
}
